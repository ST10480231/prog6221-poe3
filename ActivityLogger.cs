using System;
using System.Collections.Generic;

// ActivityLogger.cs – Tracks all user and bot actions with timestamps
namespace CybersecurityChatbotGUI
{
    public class ActivityLogger
    {
        private List<string> _logs = new List<string>();

        public void AddLog(string action)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _logs.Add($"[{timestamp}] {action}");
        }

        public List<string> GetRecentLogs(int count)
        {
            int start = Math.Max(0, _logs.Count - count);
            var result = new List<string>();
            for (int i = start; i < _logs.Count; i++)
            {
                result.Add(_logs[i]);
            }
            return result;
        }

        public string GetLogSummary()
        {
            if (_logs.Count == 0)
                return "No activity recorded yet.";

            string summary = "Here's a summary of recent actions:\n";
            int start = Math.Max(0, _logs.Count - 10);
            for (int i = start; i < _logs.Count; i++)
            {
                summary += $"• {_logs[i]}\n";
            }
            return summary;
        }

        public List<string> GetAllLogs()
        {
            return new List<string>(_logs);
        }

        public void ClearLogs()
        {
            _logs.Clear();
        }
    }
}