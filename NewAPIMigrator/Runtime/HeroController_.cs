using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class HeroController_
    {
        public static void orig_StartMPDrain(HeroControllerR self, float t)
        {
            self.StartMPDrain(t);
        }

        public static IEnumerator Invulnerable(HeroControllerR self, float t)
        {
            self.invulnerableDuration = t;
            return self.Invulnerable();
        }

        public static VibrationDataR get_softLandVibration(HeroControllerR self)
        {
            return self.vibrationCtrl.softLandVibration.vibrationData;
        }
        public static void set_softLandVibration(HeroControllerR self, VibrationDataR val)
        {
            self.vibrationCtrl.softLandVibration.vibrationData = val;
        }
        public static VibrationDataR get_shadowDashVibration(HeroControllerR self)
        {
            return self.vibrationCtrl.shadowDashVibration.vibrationData;
        }
        public static void set_shadowDashVibration(HeroControllerR self, VibrationDataR val)
        {
            self.vibrationCtrl.shadowDashVibration.vibrationData = val;
        }
        public static VibrationDataR get_doubleJumpVibration(HeroControllerR self)
        {
            return self.vibrationCtrl.doubleJumpVibration.vibrationData;
        }
        public static void set_doubleJumpVibration(HeroControllerR self, VibrationDataR val)
        {
            self.vibrationCtrl.doubleJumpVibration.vibrationData = val;
        }
        public static VibrationPlayerR get_wallSlideVibrationPlayer(HeroControllerR self)
        {
            return self.vibrationCtrl.wallSlideVibrationPlayer;
        }
        public static void set_wallSlideVibrationPlayer(HeroControllerR self, VibrationPlayerR val)
        {
            self.vibrationCtrl.wallSlideVibrationPlayer = val;
        }
        public static VibrationDataR get_wallJumpVibration(HeroControllerR self)
        {
            return default;
        }
        public static void set_wallJumpVibration(HeroControllerR self, VibrationData val)
        {

        }
        public static VibrationDataR get_dashVibration(HeroControllerR self)
        {
            return default;
        }
        public static void set_dashVibration(HeroControllerR self, VibrationData val)
        {

        }
    }
}
