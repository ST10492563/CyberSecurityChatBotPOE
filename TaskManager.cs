using System.Collections.Generic;

namespace CyberSecurtyChatBotPart2
{
    public class TaskManager
    {
        public List<CyberTask> Tasks { get; set; }

        public TaskManager()
        {
            Tasks = new List<CyberTask>();
        }

        public void AddTask(CyberTask task)
        {
            Tasks.Add(task);
        }

        public void DeleteTask(CyberTask task)
        {
            Tasks.Remove(task);
        }
    }
}