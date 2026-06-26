using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityChatbotGUI
{
    public class ChatbotEngine
    {
        private Dictionary<string, string> _responses;
        private Dictionary<string, List<string>> _randomTips;
        private Random _random = new Random();
        private string _lastTopic = "";
        private string _userName = "";
        private string _favoriteTopic = "";
        private List<string> _activityLog = new List<string>();

        public ChatbotEngine()
        {
            LoadResponses();
            LoadRandomTips();
        }

        private void LoadResponses()
        {
            _responses = new Dictionary<string, string>()
            {
                // IMPROVED PASSWORD RESPONSE FOR COMMIT 5
                { "password", "Use a strong password with at least 12 characters, mix of letters, numbers, and symbols. Never reuse passwords! Use a mix of characters for better security." },
                { "scam", "Scammers often create a sense of urgency. Always verify the sender. Never share personal info over email." },
                { "privacy", "Review your privacy settings on social media. Limit what you share publicly." },
                { "phishing", "" },
                { "virus", "Keep your antivirus software updated and avoid downloading files from unknown sources." },
                { "2fa", "Two-factor authentication adds an extra layer of security. Enable it whenever possible." },
                { "hacked", "If you think you've been hacked: change passwords immediately, run antivirus, and contact your bank." },
                { "help", "You can ask about: passwords, phishing, scams, privacy, virus, 2fa, hacked. Also tell me your name or what you're interested in." }
            };
        }

        private void LoadRandomTips()
        {
            _randomTips = new Dictionary<string, List<string>>()
            {
                {
                    "phishing", new List<string>
                    {
                        "Check the sender's email address carefully – scammers use fake addresses.",
                        "Hover over links to see the real URL before clicking.",
                        "Never download attachments from emails you weren't expecting.",
                        "Look for spelling and grammar mistakes – they're common in phishing emails.",
                        "If an email asks for personal info, contact the company directly using a known phone number.",
                        "Enable spam filters and report suspicious emails to your IT department or email provider."
                    }
                },
                {
                    "password", new List<string>
                    {
                        "Use a password manager like Bitwarden or LastPass.",
                        "Avoid using personal info like your name or birthdate.",
                        "Change your passwords every 3-6 months.",
                        "Never share your password with anyone.",
                        "Enable two-factor authentication (2FA) on all important accounts."
                    }
                }
            };
        }

        public string GetResponse(string userInput)
        {
            string input = userInput.ToLower().Trim();

            if (input.Contains("my name is"))
            {
                string name = ExtractAfterPhrase(input, "my name is");
                if (!string.IsNullOrEmpty(name))
                {
                    _userName = name;
                    _activityLog.Add($"User introduced themselves as: {name}");
                    return $"Nice to meet you, {_userName}! I'll remember that. What cybersecurity topic interests you?";
                }
            }

            if (input.Contains("interested in"))
            {
                string topic = ExtractAfterPhrase(input, "interested in");
                if (!string.IsNullOrEmpty(topic))
                {
                    _favoriteTopic = topic;
                    _activityLog.Add($"User interested in: {topic}");
                    return $"Great! I'll remember that you're interested in {_favoriteTopic}. As someone interested in that, you should always stay updated on best practices.";
                }
            }

            string sentimentPrefix = DetectSentimentAndGetPrefix(input);

            if (IsFollowUpRequest(input) && !string.IsNullOrEmpty(_lastTopic))
            {
                string followUpTip = GetRandomTipForTopic(_lastTopic);
                if (!string.IsNullOrEmpty(followUpTip))
                    return sentimentPrefix + followUpTip;
                else
                    return sentimentPrefix + (_responses.ContainsKey(_lastTopic) ? _responses[_lastTopic] : "I don't have more info on that topic right now.");
            }

            foreach (var kvp in _responses)
            {
                if (input.Contains(kvp.Key))
                {
                    _lastTopic = kvp.Key;
                    if (kvp.Key == "help")
                        return sentimentPrefix + kvp.Value;

                    string randomTip = GetRandomTipForTopic(kvp.Key);
                    if (!string.IsNullOrEmpty(randomTip))
                        return sentimentPrefix + randomTip;
                    else
                        return sentimentPrefix + kvp.Value;
                }
            }

            if (!string.IsNullOrEmpty(_favoriteTopic) && (input.Contains("tell me") || input.Contains("advice")))
            {
                return sentimentPrefix + $"Since you're interested in {_favoriteTopic}, here's a tip: " + GetRandomTipForTopic(_favoriteTopic);
            }

            return "I'm not sure I understand. Can you try rephrasing? You can ask about passwords, phishing, scams, privacy, or 2FA. Type 'help' for a list of topics.";
        }

        private string ExtractAfterPhrase(string input, string phrase)
        {
            int idx = input.IndexOf(phrase);
            if (idx >= 0)
            {
                string rest = input.Substring(idx + phrase.Length).Trim();
                string[] words = rest.Split(' ');
                if (words.Length > 0)
                    return words[0];
            }
            return "";
        }

        private bool IsFollowUpRequest(string input)
        {
            return input.Contains("tell me more") || input.Contains("another tip") ||
                   input.Contains("explain") || input.Contains("more info") ||
                   input.Contains("elaborate");
        }

        private string GetRandomTipForTopic(string topic)
        {
            if (_randomTips.ContainsKey(topic) && _randomTips[topic].Count > 0)
            {
                int index = _random.Next(_randomTips[topic].Count);
                return _randomTips[topic][index];
            }
            return "";
        }

        private string DetectSentimentAndGetPrefix(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("nervous") || input.Contains("anxious"))
                return "It's completely understandable to feel worried. Let me help you. ";
            if (input.Contains("frustrated") || input.Contains("confusing") || input.Contains("difficult"))
                return "I'm sorry you feel that way. Let me try to simplify it for you. ";
            if (input.Contains("curious") || input.Contains("want to learn") || input.Contains("tell me everything"))
                return "That's great that you're curious! Here's something useful: ";
            return "";
        }

        public List<string> GetActivityLog()
        {
            return _activityLog;
        }
    }
}
// End of ChatbotEngine – POE complete