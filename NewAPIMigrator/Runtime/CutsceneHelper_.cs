using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class CutsceneHelper_
    {
        public static IEnumerator SkipCutscene(CutsceneHelper helper)
        {
            return helper.Skip();
        }
    }
}
