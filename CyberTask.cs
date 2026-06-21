using System;

namespace CyberSecurtyChatBotPart2
{
    public class CyberTask
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime ReminderDate { get; set; }

        public bool IsCompleted { get; set; }

        public CyberTask()
        {
            IsCompleted = false;
        }
    }
}
