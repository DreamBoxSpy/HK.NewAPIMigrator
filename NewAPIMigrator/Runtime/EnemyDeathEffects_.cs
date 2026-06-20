using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class EnemyDeathEffects_
    {
        public static void orig_RecieveDeathEvent(EnemyDeathEffectsR self, float? dir, bool b1, bool b2, bool b3)
        {
            self.RecieveDeathEvent(dir, b1, b2, b3);
        }
        public static void orig_RecordKillForJournal(EnemyDeathEffectsR self)
        {
            self.RecordKillForJournal();
        }
    }
}
