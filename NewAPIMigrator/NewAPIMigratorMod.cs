using HKTool;
using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace NewAPIMigrator
{
    public class NewAPIMigratorMod : Modding.Mod
    {
        public NewAPIMigratorMod() : base("New API Migrator")
        {
            if(!InitManager.IsInit)
            {
                LogWarn("Migrator is not installed.");
                Log("Installing migrator...");
                InitManager.InstallInit();

                Log("Restart game.");

                Process.Start(Process.GetCurrentProcess().ProcessName);
                Application.Quit();
            }

            Log("Migrator installed.");
        }

        public override string GetVersion()
        {
            return typeof(NewAPIMigratorMod).Assembly.GetName().Version.ToString();
        }
    }
}
