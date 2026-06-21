using MySql.Data.MySqlClient;
using CyberSecurityBotPart2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace CyberSecurtyChatBotPart2
{
    public partial class MainWindow : Window
    {
        private ChatMemory memory = new ChatMemory();

        private Random random = new Random();

        private List<Brush> backgrounds = new List<Brush>()
        {
            Brushes.DarkSlateBlue,
            Brushes.DarkOliveGreen,
            Brushes.DarkCyan,
            Brushes.DarkRed,
            Brushes.MidnightBlue
        };

        private Dictionary<string, List<string>> cyberResponses =
            new Dictionary<string, List<string>>()
        {
            {
                "password",
                new List<string>()
                {
                    "Use strong passwords with symbols.",
                    "Never reuse passwords on different websites.",
                    "Enable two-factor authentication for extra protection."
                }
            },

            {
                "privacy",
                new List<string>()
                {
                    "Avoid sharing personal information publicly.",
                    "Review app permissions regularly.",
                    "Use privacy settings on social media."
                }
            },

            {
                "scam",
                new List<string>()
                {
                    "Never trust suspicious emails.",
                    "Scammers often create urgency to trick victims.",
                    "Verify links before clicking them."
                }
            },

            {
                "phishing",
                new List<string>()
                {
                    "Avoid clicking suspicious links.",
                    "Check email addresses carefully.",
                    "Do not download unknown attachments.",
                    "Use antivirus software."
                }
            }
        };

        private string historyFile = "History/chat_history.txt";

        private bool nameStored = false;

        private string connectionString =
    "server=localhost;user=root;password=Kuhlula;database=CyberBot;";

        public MainWindow()
        {
            InitializeComponent();

            StartBackgroundAnimation();

            PlayGreeting();

            AddBotMessage("Hello, I am an Online Cyberbot Molly. What is your name?");
        }

        // BACKGROUND ANIMATION
        private void StartBackgroundAnimation()
        {
            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromSeconds(5);

            timer.Tick += (s, e) =>
            {
                MainGrid.Background =
                    backgrounds[random.Next(backgrounds.Count)];
            };

            timer.Start();
        }

        // GREETING SOUND
        private void PlayGreeting()
        {
            try
            {
                SoundPlayer player =
                    new SoundPlayer("Welcom.wav");

                player.Play();
            }
            catch
            {
                MessageBox.Show("Greeting sound not found.");
            }
        }

        // SEND BUTTON
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = UserInput.Text.Trim();

                if (string.IsNullOrWhiteSpace(message))
                {
                    AddBotMessage("Please type something.");
                    return;
                }

                AddUserMessage(message);

                SaveMessage("USER", message);

                UserInput.Clear();

                string lower = message.ToLower();

                // STORE NAME
                if (!nameStored)
                {
                    memory.UserMemory["name"] = message;

                    nameStored = true;

                    await TypingAnimation();

                    AddBotMessage("Nice to meet you " + message);

                    return;
                }

                await TypingAnimation();

                // NAME MEMORY
                if (lower.Contains("what is my name"))
                {
                    AddBotMessage("Your name is " +
                        memory.UserMemory["name"]);
                }

                // EMOTIONS
                else if (lower.Contains("sad"))
                {
                    AddBotMessage("I'm sorry you're sad.");
                }

                else if (lower.Contains("happy"))
                {
                    AddBotMessage("That's amazing.");
                }

                // FOLLOW-UP
                else if (lower.Contains("tell me more") ||
                         lower.Contains("another tip") ||
                         lower.Contains("explain more"))
                {
                    HandleFollowUp();
                }

                // KEYWORD RESPONSES
                else
                {
                    bool found = false;

                    foreach (var keyword in cyberResponses.Keys)
                    {
                        if (lower.Contains(keyword))
                        {
                            memory.LastTopic = keyword;

                            var responses =
                                cyberResponses[keyword];

                            string randomResponse =
                                responses[random.Next(responses.Count)];

                            AddBotMessage(randomResponse);

                            found = true;

                            break;
                        }
                    }

                    if (!found)
                    {
                        AddBotMessage("I am still learning about that topic.");
                    }
                }
            }
            catch (Exception ex)
            {
                AddBotMessage("Error: " + ex.Message);
            }
        }

        // FOLLOW-UP SYSTEM
        private void HandleFollowUp()
        {
            if (memory.LastTopic != null &&
                cyberResponses.ContainsKey(memory.LastTopic))
            {
                var responses =
                    cyberResponses[memory.LastTopic];

                string randomResponse =
                    responses[random.Next(responses.Count)];

                AddBotMessage(randomResponse);
            }
            else
            {
                AddBotMessage("Please ask about a topic first.");
            }
        }

        // BOT MESSAGE
        private void AddBotMessage(string message)
        {
            StackPanel stack = new StackPanel();

            TextBlock time = new TextBlock()
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = Brushes.LightGray,
                FontSize = 11
            };

            Border border = new Border()
            {
                Background = Brushes.DarkSlateBlue,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(12),
                Margin = new Thickness(5),
                MaxWidth = 420,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            TextBlock text = new TextBlock()
            {
                Text = "Molly: " + message,
                Foreground = Brushes.White,
                FontSize = 16,
                TextWrapping = TextWrapping.Wrap
            };

            border.Child = text;

            stack.Children.Add(time);
            stack.Children.Add(border);

            ChatPanel.Children.Add(stack);
        }

        // USER MESSAGE
        private void AddUserMessage(string message)
        {
            StackPanel stack = new StackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Right
            };

            TextBlock time = new TextBlock()
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Foreground = Brushes.LightGray,
                FontSize = 11,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Border border = new Border()
            {
                Background = Brushes.Teal,
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(12),
                Margin = new Thickness(5),
                MaxWidth = 420,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            TextBlock text = new TextBlock()
            {
                Text = "You: " + message,
                Foreground = Brushes.White,
                FontSize = 16,
                TextWrapping = TextWrapping.Wrap
            };

            border.Child = text;

            stack.Children.Add(time);
            stack.Children.Add(border);

            ChatPanel.Children.Add(stack);
        }

        // TYPING EFFECT
        private async Task TypingAnimation()
        {
            Border typingBorder = new Border()
            {
                Background = Brushes.Gray,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            TextBlock typingText = new TextBlock()
            {
                Text = "Molly is typing...",
                Foreground = Brushes.White
            };

            typingBorder.Child = typingText;

            ChatPanel.Children.Add(typingBorder);

            await Task.Delay(1200);

            ChatPanel.Children.Remove(typingBorder);
        }

        // SAVE CHAT
        private void SaveMessage(string sender, string message)
        {
            Directory.CreateDirectory("History");

            string line =
                $"{DateTime.Now} [{sender}] {message}";

            File.AppendAllText(
                historyFile,
                line + Environment.NewLine);
        }

        // TASK BUTTON
        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddBotMessage("=== TASK LIST ===");

            if (memory.Tasks.Count == 0)
            {
                AddBotMessage("No tasks available.");
                return;
            }

            foreach (var task in memory.Tasks)
            {
                AddBotMessage(
                    $"Task: {task.Title}\n" +
                    $"Description: {task.Description}\n" +
                    $"Reminder: {task.ReminderDate}\n" +
                    $"Completed: {task.IsCompleted}");
            }

            memory.AddActivity("Viewed task list");
        }

        // QUIZ BUTTON
        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            AddBotMessage("Cybersecurity Quiz Started!");

            AddBotMessage(
                "Question 1:\n" +
                "What is phishing?\n\n" +
                "A) A cyber attack\n" +
                "B) A search engine\n" +
                "C) An antivirus\n" +
                "D) A firewall");

            memory.AddActivity("Quiz started");
        }

        // ACTIVITY LOG BUTTON
        private void ActivityButton_Click(object sender, RoutedEventArgs e)
        {
            AddBotMessage("=== ACTIVITY LOG ===");

            if (memory.ActivityLog.Count == 0)
            {
                AddBotMessage("No activity recorded.");
                return;
            }

            foreach (string activity in memory.ActivityLog)
            {
                AddBotMessage(activity);
            }
        }

        // CLEAR CHAT BUTTON
        private void ClearChatButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Children.Clear();

            AddBotMessage("Chat cleared.");

            memory.AddActivity("Chat cleared");
        }

        // ADD TASK METHOD
        private void AddTask(string title, string description, DateTime reminder)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted) " +
                               "VALUES (@title, @desc, @date, @done)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@date", reminder);
                cmd.Parameters.AddWithValue("@done", false);

                cmd.ExecuteNonQuery();
            }

            AddBotMessage("Task saved to database!");
        }

        private void LoadTasks()
        {
            memory.Tasks.Clear();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Tasks";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        memory.Tasks.Add(new CyberTask
                        {
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            ReminderDate = Convert.ToDateTime(reader["ReminderDate"]),
                            IsCompleted = Convert.ToBoolean(reader["IsCompleted"])
                        });
                    }
                }
            }
        }

        // COMPLETE TASK METHOD
        private void CompleteTask(string title)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE Tasks SET IsCompleted = 1 WHERE Title = @title";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", title);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    AddBotMessage("Task marked as completed in database.");
                else
                    AddBotMessage("Task not found.");
            }
        }

        // DELETE TASK METHOD
        private void DeleteTask(string title)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM Tasks WHERE Title = @title";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", title);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    AddBotMessage("Task deleted from database.");
                else
                    AddBotMessage("Task not found.");
            }
        }

        // SHOW ACTIVITY LOG METHOD
        private void ShowActivityLog()
        {
            AddBotMessage("=== ACTIVITY LOG ===");

            foreach (var item in memory.ActivityLog)
            {
                AddBotMessage(item);
            }
        }
    }
}


