using System.Collections.Generic;

namespace CyberSecurtyChatBotPart2
{
    public class QuizQuestion
    {
        public string Question { get; set; }

        public List<string> Options { get; set; }

        public string CorrectAnswer { get; set; }

        public QuizQuestion()
        {
            Options = new List<string>();
        }
    }
}