using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class CameraLockArea_
    {
        public static IEnumerator orig_Start(CameraLockArea self)
        {
            return self.Reflect().Start();
        }
    }
}
