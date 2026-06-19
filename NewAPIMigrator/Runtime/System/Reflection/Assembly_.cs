using NewAPIMigrator.Migrator;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NewAPIMigrator.Runtime.System.Reflection
{
    public static class Assembly_
    {
        [MigrateIgnore]
        public static string get_Location(Assembly assembly)
        {
            if(MigratorManager.Instance.locationMapping.TryGetValue(assembly, out var location))
            {
                return location;
            }
            return assembly.Location;
        }
    }
}
