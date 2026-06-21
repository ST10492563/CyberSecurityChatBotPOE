using System;
using System.Collections.Generic;

namespace CyberSecurtyChatBotPart2
{
    public class ActivityLog
    {
        private List<string> activities =
            new List<string>();

        public void Add(string activity)
        {
            activities.Add(
                DateTime.Now.ToString("g")
                + " - "
                + activity);
        }

        public List<string> GetActivities()
        {
            return activities;
        }
    }
}