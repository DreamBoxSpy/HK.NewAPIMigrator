using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class VibrationMixer_
    {
        public static VibrationEmissionR PlayEmission(VibrationMixerR self, VibrationDataR data, VibrationTargetR target, bool b1, string s1)
        {
            return self.PlayEmission(data, target, b1, s1, false);
        }
    }
}
