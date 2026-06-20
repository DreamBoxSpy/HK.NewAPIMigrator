using Modding;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace NewAPIMigrator.Migrator
{
    public partial class MigratorManager
    {
        public readonly UnityEngine.ILogger logger = UnityEngine.Debug.unityLogger;

        public static MigratorManager Instance { get; private set; } = null!;

        private readonly List<string> modDLLs = [];

        private readonly Dictionary<string, Type> typeMapping = [];

        public readonly string MODS_ROOT = Path.GetFullPath(
            Path.Combine(typeof(MigratorManager).Assembly.Location, "..", "..")
            );

        public readonly string CACHE_ROOT = Path.GetFullPath(
            Path.Combine(typeof(MigratorManager).Assembly.Location, "..", "old_mods_cache")
            );

        public readonly Dictionary<Assembly, string> locationMapping = [];

        public MigratorManager()
        {
            Instance = this;

            Directory.CreateDirectory(CACHE_ROOT);

            var hash = BitConverter.ToString(SHA256.Create().ComputeHash(File.ReadAllBytes(typeof(MigratorManager).Assembly.Location))).Replace("-", "")[..64];

            var hp = Path.Combine(CACHE_ROOT, "ver-" + hash);
            if(!File.Exists(hp))
            {
                Directory.Delete(CACHE_ROOT, true);
                Directory.CreateDirectory(CACHE_ROOT);
                File.WriteAllText(hp, hash);
            }

            foreach (var v in typeof(MigrateIgnore).Assembly.GetTypes())
            {
                var fn = v.FullName;
                if(fn.StartsWith("NewAPIMigrator.Runtime."))
                {
                    typeMapping.Add(fn, v);
                }
            }

            InitRemap();
        }

        private string GetRuntimeName(string fullname)
        {
            return $"NewAPIMigrator.Runtime.{fullname}_";
        }

        private Type? GetRuntimeType(TypeReference tr)
        {
            var fn = tr.FullName;
            if(typeMapping.TryGetValue(GetRuntimeName(fn), out var result))
            {
                return result;
            }
            return null;
        }

        public void Start()
        {

            logger.Log("Start migrating...");

            logger.Log($"Cache path: {CACHE_ROOT}");

            logger.Log("Scanning mods...");
            ScanMods();

            logger.Log("Migrating mods...");

            foreach(var v in modDLLs)
            {
                try
                {
                    MigrateMod(v);
                }catch(Exception ex)
                {
                    logger.LogException(ex);
                }
            }

        }

        private void MigrateMod(string path)
        {
            logger.Log("Migrating mod: " + path);

            var data = File.ReadAllBytes(path);
            var hash = BitConverter.ToString(SHA256.Create().ComputeHash(data)).Replace("-", "")[..64];

            var cachePath = Path.Combine(CACHE_ROOT, hash + ".dll");
            var noCachePath = cachePath + ".no";

            if(File.Exists(noCachePath))
            { 
                logger.Log("No Cache. Skip.");
                return;
            }

            if(File.Exists(cachePath))
            {
                logger.Log("Found cache.");
                var asm = Assembly.Load(File.ReadAllBytes(cachePath));
                locationMapping[asm] = path;
                return;
            }

            var stream = new MemoryStream(data, true);
            using var resolver = new DefaultAssemblyResolver();

            foreach(var v in modDLLs.Select(x => Path.GetDirectoryName(x)).Distinct())
            {
                resolver.AddSearchDirectory(v);
            }

            using var ad = AssemblyDefinition.ReadAssembly(stream, new()
            {
                AssemblyResolver = resolver
            });

            var migrated = false;

            migrated |= RemapReferences(ad);
            migrated |= RedirectReferences(ad);

            if(migrated)
            {
                stream.Position = 0;

                var ms = new MemoryStream();
                ad.Write(ms);

                var dat = ms.ToArray();

                File.WriteAllBytes(cachePath, dat);

                var asm = Assembly.Load(dat);
                locationMapping[asm] = path;

                logger.Log("Mirgated.");
                return;
            }

            File.WriteAllText(noCachePath, "no cache");
            logger.Log("Skip.");
        }


        private void ScanMods()
        {
            List<string> dirs = [];
            foreach(var v in Directory.EnumerateDirectories(MODS_ROOT, "*", SearchOption.TopDirectoryOnly))
            {
                if(v.Equals("Disabled", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                dirs.Add(v);
            }
            foreach (var v in dirs.SelectMany(x => Directory.EnumerateFiles(x, "*.dll", SearchOption.TopDirectoryOnly)))
            {
                logger.Log("Mod DLL: " + v);
                modDLLs.Add(v);
            }
        }
    }
}
