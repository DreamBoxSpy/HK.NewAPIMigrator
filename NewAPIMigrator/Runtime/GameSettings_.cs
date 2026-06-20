using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class GameSettings_
    {
        public static bool LoadInt(GameSettingsR self, string key, ref int ri, int i)
        {
            return GameSettingsR.LoadInt(key, ref ri, i);
        }
        public static bool HasSetting(GameSettingsR self, string key)
        {
            return GameSettingsR.HasSetting(key);
        }
        public static bool LoadEnum<TE>(GameSettingsR self, string key, ref TE e, TE e2)
        {
            return GameSettingsR.LoadEnum(key, ref e, e2);
        }
        public static bool LoadBool(GameSettingsR self, string key, ref bool b1, bool b2)
        {
            return GameSettingsR.LoadBool(key, ref b1, b2);
        }
        public static bool LoadFloat(GameSettings self, string key, ref float f1, float f2)
        {
            return GameSettingsR.LoadFloat(key, ref f1, f2);
        }
        public static bool LoadString(GameSettingsR self, string key, ref string s1, string s2)
        {
            return GameSettingsR.LoadString(key, ref s1, s2);
        }
    }
}
