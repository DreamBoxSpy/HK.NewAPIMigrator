using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime.UnityEngine
{
    public class HeroController_
    {
        public static void orig_StartMPDrain(HeroController self, float val)
        {
            self.Reflect().StartMPDrain(val);
        }

        public static IEnumerator Invulnerable(HeroController self, float time)
        {
            self.Reflect().invulnerableDuration = time;
            return self.Reflect().Invulnerable();
        }

        public static void orig_Update(HeroController self)
        {
            self.Reflect().Update();
        }

        public static void orig_Dash(HeroController self)
        {
            self.Reflect().Dash();
        }

        public static void orig_DoAttack(HeroController self)
        {
            self.Reflect().DoAttack();
        }
    }
}
