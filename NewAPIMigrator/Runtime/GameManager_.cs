using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class GameManager_
    {
        public static void orig_LoadScene(GameManagerR self, string name)
        {
            self.LoadScene(name);
        }

        public static void orig_ClearSaveFile(GameManagerR self, int slot, Action<bool> cb)
        {
            self.ClearSaveFile(slot, cb);
        }

        public static void orig_PlayerDead(GameManagerR self, float t)
        {
            self.PlayerDead(t);
        }

        public static void orig_OnWillActivateFirstLevel(GameManagerR self)
        {
            self.OnWillActivateFirstLevel();
        }
    }
}
