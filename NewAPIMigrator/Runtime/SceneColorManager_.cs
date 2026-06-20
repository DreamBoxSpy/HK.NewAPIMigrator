using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class SceneColorManager_
    {
        public static bool get_markerActive(SceneColorManagerR self)
        {
            return self.MarkerActive;
        }
        public static void set_markerActive(SceneColorManagerR self, bool val)
        {
            self.MarkerActive = val;
        }
        public static bool get_startBufferActive(SceneColorManagerR self)
        {
            return self.StartBufferActive;
        }
        public static void set_startBufferActive(SceneColorManagerR self, bool val)
        {
            self.StartBufferActive = val;
        }
        public static void MarkerActive(SceneColorManagerR self, bool val)
        {
            self.SetMarkerActive(val);
        }
    }
}
