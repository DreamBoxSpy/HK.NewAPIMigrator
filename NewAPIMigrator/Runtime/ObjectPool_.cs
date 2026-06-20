using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NewAPIMigrator.Runtime
{
    public  class ObjectPool_
    {
        public static void orig_CreatePool(GameObject obj, int i)
        {
            ObjectPool.CreatePool(obj, i);
        }
    }
}
