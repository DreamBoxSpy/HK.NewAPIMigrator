using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NewAPIMigrator.Runtime
{
    public class PersonalObjectPool_
    {
        public static void CreatePool(PersonalObjectPoolR self, GameObject go, int val)
        {
            self.CreatePool(go, val, true);
        }

        public static StartupPool[] get_startupPool(PersonalObjectPool self)
        {
            return [.. self.startupPool];
        }
        public static void set_startupPool(PersonalObjectPool self, StartupPool[] val)
        {
            self.startupPool = [.. val];
        }
    }
}
