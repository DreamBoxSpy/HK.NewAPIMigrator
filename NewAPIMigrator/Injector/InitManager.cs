
using HKTool.Unity;
using MonoMod.RuntimeDetour;
using NewAPIMigrator.Migrator;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace HKTool;

public class InitManager
{
    

    public static bool IsInit => _isInit;

    private static bool _isInit = false;

    private static InitManager? instance; 
    
    public MigratorManager manager;

    private Hook? hook_get_location;

    public static void InstallInit()
    {
        var ass = ScriptingAssemblies.Load();
        ass.AddAssembly(typeof(InitManager).Assembly.Location);
        ass.Save();

        var init = RuntimeInitializeOnLoads.Load();
        if (init.root.Any(x => x.assemblyName == "NewAPIMigrator" && x.nameSpace == "HKTool" && x.className == "InitManager" &&
            x.methodName == nameof(CheckInit))) return;
        init.root.Add(new()
        {
            assemblyName = "NewAPIMigrator",
            nameSpace = "HKTool",
            className = "InitManager",
            methodName = nameof(CheckInit)
        });
        init.Save();
    }
    public static void UninstallInit()
    {
        var ass = ScriptingAssemblies.Load();
        ass.Remove(typeof(InitManager).Assembly.Location);
        ass.Save();

        var init = RuntimeInitializeOnLoads.Load();
        init.root.RemoveAll(x => x.assemblyName == "NewAPIMigrator");
        init.Save();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static void CheckInit()
    {
        if (_isInit) return;
        _isInit = true;

        instance = new();
        instance.Init();

    }

    private void Init()
    {
        manager = new MigratorManager();
        manager.Start();

        hook_get_location = new(Type.GetType("System.Reflection.RuntimeAssembly,mscorlib", true)
            .GetProperty("Location")
            .GetMethod, Hook_Assembly_get_Location
            );
        hook_get_location.Apply();
    }

    private static string Hook_Assembly_get_Location(Func<Assembly, string> orig, Assembly asm)
    {
        if(instance!.manager.locationMapping.TryGetValue(asm, out var loc))
        {
            return loc;
        }
        return orig(asm);
    }
}

