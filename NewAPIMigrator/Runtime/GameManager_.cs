using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class GameManager_
    {
        public static void orig_LoadScene(GameManagerR self, string name)
        {
            using var _ = new ModHooksManager.DisableModHooks();
            self.LoadScene(name);
        }

        public static void orig_ClearSaveFile(GameManagerR self, int slot, Action<bool> cb)
        {
            using var _ = new ModHooksManager.DisableModHooks();
            self.ClearSaveFile(slot, cb);
        }

        public static void orig_PlayerDead(GameManagerR self, float t)
        {
            using var _ = new ModHooksManager.DisableModHooks();
            self.PlayerDead(t);
        }

        public static void orig_OnWillActivateFirstLevel(GameManagerR self)
        {
            using var _ = new ModHooksManager.DisableModHooks();
            self.OnWillActivateFirstLevel();
        }
    }
}
