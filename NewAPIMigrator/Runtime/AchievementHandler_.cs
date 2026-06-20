using System;
using System.Collections.Generic;
using System.Text;

namespace NewAPIMigrator.Runtime
{
    public class AchievementHandler_
    {
        public static AchievementsListR get_achievementsList(AchievementHandlerR self)
        {
            return self.AchievementsList;
        }
        public static void set_achievementsList(AchievementHandlerR self, AchievementsListR val)
        {
            self.achievementsList = val;
        }

    }
}
