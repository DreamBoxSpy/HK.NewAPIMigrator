using Modding;
using Modding.Utils;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace NewAPIMigrator.Migrator
{
    partial class MigratorManager
    {
        private readonly Dictionary<string, (AssemblyNameReference asmName, string np, string name)> remapMapping = [];
        private readonly List<AssemblyNameReference> assemblyNameReferences = [];
        private void InitRemap()
        {
            void Add(string from, string to)
            {
                remapMapping[to] = remapMapping[from];

                if (remapMapping.TryGetValue("On." + from, out var onHook))
                {
                    remapMapping["On." + to] = onHook;
                }
                if (remapMapping.TryGetValue("IL." + from, out var ilHook))
                {
                    remapMapping["IL." + to] = ilHook;
                }
            }

            void CopyNamespace(string from, string? to)
            {
                var str = from + ".";
                foreach(var v in remapMapping.Keys.ToArray())
                {
                    if(v.StartsWith(str))
                    {
                        var name = v[str.Length..];
                        if (string.IsNullOrEmpty(to))
                        {
                            Add(v, name);
                        }
                        else
                        {
                            Add(v, to + "." + name);
                        }
                        
                    }
                }
            }

            logger.Log("Loading managed assemblies...");

            var managedRoot = Path.GetDirectoryName(typeof(Application).Assembly.Location)!;

            foreach(var v in Directory.EnumerateFiles(managedRoot, "*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    Assembly.LoadFrom(v);
                }
                catch { }
            }
            
            HashSet<string> ignoreTypes = [];
            foreach (var v in AppDomain.CurrentDomain.GetAssemblies())
            {
                var an = v.GetName();
                var ar = new AssemblyNameReference(an.Name, an.Version);
                assemblyNameReferences.Add(ar);
                foreach (var type in v.GetTypes())
                {
                    var result = (ar, type.Namespace, type.Name);

                    var fn = type.FullName;

                    if(!remapMapping.TryAdd(fn, result))
                    {
                        ignoreTypes.Add(fn);
                        continue;
                    }

                    if (!fn.StartsWith("On.") && !fn.StartsWith("IL."))
                    {
                        remapMapping["TeamCherry." + fn] = result;
                    }
                }
            }

            foreach(var v in ignoreTypes)
            {
                remapMapping.Remove(v);
            }

            // TeamCherry.Localization

            CopyNamespace("TeamCherry.Localization", "Language");

            // TextMeshPro

            CopyNamespace("TMProOld", "TMPro");

            // 

            CopyNamespace("TeamCherry.Cinematics", null);

            Add("TeamCherry.Cinematics.StreamingCinematicVideoPlayer", "XB1CinematicVideoPlayer");
            Add("HutongGames.PlayMaker.Actions.SendEventToRegister", "SendEventToRegister");
            Add("HutongGames.PlayMaker.Actions.AddEventRegister", "AddEventRegister");
            Add("HutongGames.PlayMaker.Actions.PreloadVibration", "PreloadVibration");
            Add("HutongGames.PlayMaker.Actions.PlayVibration", "PlayVibration");
            Add("HutongGames.PlayMaker.Actions.StopVibration", "StopVibration");
            
        }
        private bool RemapReferences(AssemblyDefinition ad)
        {
            bool migrated = false;

            Extensions.IsAny("Test", "Test");

            foreach(var v in assemblyNameReferences)
            {
                ad.MainModule.AssemblyReferences.Add(v);
            }
            foreach(var v in ad.MainModule.GetTypeReferences())
            {
                if(!v.IsDefinition && 
                    v.DeclaringType == null &&
                    v.Scope is AssemblyNameReference nameRef)
                {
                    if(remapMapping.TryGetValue(v.FullName, out var newName))
                    {
                        if (nameRef.Name == newName.asmName.Name &&
                            v.Namespace == newName.np &&
                            v.Name == newName.name)
                        {
                            Type.GetType($"{v.FullName},{newName.asmName.Name}", true);
                            continue;
                        }

                        v.Name = newName.name;
                        v.Namespace = string.IsNullOrEmpty(newName.np) ? null : newName.np;
                        v.Scope = newName.asmName;

                        Type.GetType($"{v.FullName},{newName.asmName.Name}", true);

                        migrated = true;
                    }
                }
            }

            return migrated;
        }
    }
}
