using System;
using System.Collections.Generic;
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
    }
}
