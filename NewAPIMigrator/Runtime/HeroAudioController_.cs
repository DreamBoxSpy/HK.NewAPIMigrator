using GlobalEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class HeroAudioController_
    {
        public static void PlaySound(HeroAudioController ctrl, HeroSounds sounds)
        {
            ctrl.PlaySound(sounds);
        }
    }
}
