using Mono.Cecil;
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

            var asmRef = new AssemblyNameReference("NewAPIMigrator", new());
            assembly.MainModule.AssemblyReferences.Add(asmRef);

            foreach (var v in assembly.MainModule.GetTypeReferences().ToArray())
            {
                var vt = GetRuntimeType(v);
                if (vt == null)
                {
                    continue;
                }
                if (vt.GetCustomAttribute<RedirectType>() is not null)
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
                    vt.GetCustomAttribute<RedirectType>() is not null)
                {
                    continue;
                }
                if (v is MethodReference mr)
                {
                    var newMethod = vt.GetMethods(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                        .FirstOrDefault(x => x.Name == mr.Name);

                    if (newMethod != null)
                    {
                        var m = assembly.MainModule.ImportReference(newMethod);

                        if (mr.HasThis)
                        {
                            if (!mr.ExplicitThis)
                            {
                                mr.Parameters.Insert(0, new(mr.DeclaringType));
                            }

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
            }

            return migrated;
        }
    }
}
