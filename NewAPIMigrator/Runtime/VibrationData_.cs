using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NewAPIMigrator.Runtime
{
    public class VibrationData_
    {
        public static VibrationDataR Create(LowFidelityVibrationsR a1, TextAsset a2, GamepadVibrationR a3)
        {
            return VibrationDataR.Create(a1, a2, a3);
        }
    }
}
