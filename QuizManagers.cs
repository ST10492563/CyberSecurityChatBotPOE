using System.Collections.Generic;

namespace CyberSecurtyChatBotPart2
{
    public class QuizManager
    {
        public List<QuizQuestion> Questions { get; set; }

        public int Score { get; set; }

        public QuizManager()
        {
            Questions = new List<QuizQuestion>();

            Questions.Add(new QuizQuestion()
            {
                Question = "What is phishing?",
                Options = new List<string>()
                {
                    "A cyber attack",
                    "A browser",
                    "An antivirus",
                    "A firewall"
                },
                CorrectAnswer = "A cyber attack"
            });

            Questions.Add(new QuizQuestion()
            {
                Question = "Should you share passwords?",
                Options = new List<string>()
                {
                    "Yes",
                    "No"
                },
                CorrectAnswer = "No"
            });
        }
    }
}
