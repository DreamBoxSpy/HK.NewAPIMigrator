using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NewAPIMigrator.Migrator
{
    partial class MigratorManager
    {
       
        private bool RedirectReferences(AssemblyDefinition assembly)
        {
            var migrated = false;

            Dictionary<FieldReference, (MethodReference getter, MethodReference setter)> fieldRemap = [];

            var asmRef = new AssemblyNameReference("NewAPIMigrator", new());
            assembly.MainModule.AssemblyReferences.Add(asmRef);

            foreach (var v in assembly.MainModule.GetTypeReferences().ToArray())
            {
                var vt = GetRuntimeType(v);
                if (vt == null)
                {
                    continue;
                }
                if (vt.GetCustomAttribute<ReplaceType>() is not null)
                {
                    if (vt.GetCustomAttribute<MigrateIgnore>() is null)
                    {
                        migrated = true;
                    }
                    v.Namespace = vt.Namespace;
                    v.Name = vt.Name;
                    v.Scope = asmRef;
                }

            }

            foreach (var v in assembly.MainModule.GetMemberReferences().ToArray())
            {
                var pt = v.DeclaringType;
                var vt = GetRuntimeType(pt);
                if (vt == null ||
                    vt.GetCustomAttribute<ReplaceType>() is not null)
                {
                    continue;
                }
                if (v is MethodReference mr)
                {
                    var newMethod = vt.GetMethods(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                            .FirstOrDefault(x => x.Name == mr.Name) ?? 
                        vt.GetMethods(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                            .FirstOrDefault(x => x.Name == mr.Name + "_" + mr.Parameters.Count);
                    if (newMethod != null)
                    {
                        var m = assembly.MainModule.ImportReference(newMethod);

                        if (mr.HasThis)
                        {
                            if (!mr.ExplicitThis)
                            {
                                mr.Parameters.Insert(0, new(mr.DeclaringType));
                            }

                            mr.Name = newMethod.Name;
                            mr.HasThis = false;
                            mr.ExplicitThis = false; 
                        }

                        mr.DeclaringType = m.DeclaringType;

                        if (vt.GetCustomAttribute<MigrateIgnore>() is null)
                        {
                            migrated = true;
                        }
                    }
                }
                else if(v is FieldReference fr)
                {
                    var getter = vt.GetMethod("get_" + fr.Name, BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
                    var setter = vt.GetMethod("set_" + fr.Name, BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);

                    if(getter == null || setter == null)
                    {
                        continue;
                    }

                    fieldRemap[fr] = (assembly.MainModule.ImportReference(getter), assembly.MainModule.ImportReference(setter));
                }
                
            }

            if(fieldRemap.Count > 0)
            {
                migrated = true;

                foreach(var t in assembly.MainModule.GetTypes())
                {
                    foreach(var m in t.Methods)
                    {
                        if(m.Body == null || m.Body.Instructions == null)
                        {
                            continue;
                        }

                        foreach(var inst in m.Body.Instructions)
                        {
                            if(inst.Operand is FieldReference fr)
                            {
                                if(!fieldRemap.TryGetValue(fr, out var fieldInfo))
                                {
                                    continue;
                                }

                                if(inst.OpCode == OpCodes.Ldfld || inst.OpCode == OpCodes.Ldsfld)
                                {
                                    inst.OpCode = OpCodes.Call;
                                    inst.Operand = fieldInfo.getter;
                                }
                                else if(inst.OpCode == OpCodes.Stfld || inst.OpCode == OpCodes.Stsfld)
                                {
                                    inst.OpCode = OpCodes.Call;
                                    inst.Operand = fieldInfo.setter;
                                }
                            }
                        }
                    }
                }
            }

            return migrated;
        }
    }
}
