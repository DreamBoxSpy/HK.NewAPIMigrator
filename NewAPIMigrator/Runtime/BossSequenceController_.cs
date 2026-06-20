using System;
using System.Collections.Generic;
using System.Text;
using static BossSequenceController;

namespace NewAPIMigrator.Runtime
{
    public class BossSequenceController_
    {
        public static void SetupNewSequence(BossSequenceR seq, ChallengeBindingsR c, string s)
        {
            BossSequenceControllerR.SetupNewSequence(seq, c, s);
        }
    }
}
