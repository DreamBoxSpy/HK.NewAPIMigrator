using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class VibrationManager_
    {
        private static bool isMuted;
        public static bool get_IsMuted()
        {
            return isMuted;
        }
        public static void set_IsMuted(bool val)
        {
            isMuted = val;
        }
        public static VibrationEmissionR PlayVibrationClipOneShot(VibrationDataR data, VibrationTargetR? target,  bool b1, string s1)
        {
            return VibrationManagerR.PlayVibrationClipOneShot(data, target, b1, s1);
        }
    }
}
