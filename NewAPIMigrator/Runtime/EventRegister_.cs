using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public static class EventRegister_
    {
        public static void SendEvent(string ev)
        {
            EventRegister.SendEvent(ev, null);
        }
    }
}
