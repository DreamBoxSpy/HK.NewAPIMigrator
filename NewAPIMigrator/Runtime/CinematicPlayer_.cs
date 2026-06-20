using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class CinematicPlayer_
    {
        public static IEnumerator SkipVideo(CinematicPlayer player)
        {
            return player.Skip();
        }
    }
}
