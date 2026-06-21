using System;
using System.Collections.Generic;

namespace CyberSecurtyChatBotPart2
{
    internal class ChatMemory
    {
        public Dictionary<string, string> UserMemory =
            new Dictionary<string, string>();

        public string LastTopic { get; set; }

        public List<CyberTask> Tasks =
            new List<CyberTask>();

        public List<string> ActivityLog =
            new List<string>();

        public int QuizScore { get; set; }

        public void AddActivity(string activity)
        {
            ActivityLog.Add(
                DateTime.Now.ToString("g") + " - " + activity);
        }
    }
}
