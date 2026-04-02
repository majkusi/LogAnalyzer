using LogAnalyzer.Core.Entities.Enums;
using LogAnalyzer.Core.Entities.LogEntryAggregate;
using System.Text.RegularExpressions;

namespace LogAnalyzer.Core.Services
{
    public class LogParserEngine
    {
        private static readonly Regex LogRegex = new Regex(
        @"^(?<timestamp>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2})\s\[(?<level>\w+)\]\s(?<source>[\w\.]+):\s(?<message>.*)$",
        RegexOptions.Compiled | RegexOptions.Multiline);

        private static readonly Regex StackTraceRegex = new Regex(
        @"^\s+at\s+(?<method>.*)\s+in\s+(?<file>.*?):line\s+(?<line>\d+)$",
        RegexOptions.Compiled | RegexOptions.Multiline);

        public List<LogEntry> ParseLogs(string logsFilePath)
        {
            if (!File.Exists(logsFilePath))
                throw new ArgumentException("Log file path is incorrect or file does not exist!");

            bool isCollectingNewStackTraceBlock = false;
            var logFile = File.ReadLines(logsFilePath);
            List<LogEntry> listOfLogEntries = new List<LogEntry>();
            var newLogEntry = new LogEntry();

            FileParserHelper(ref isCollectingNewStackTraceBlock, logFile, listOfLogEntries, ref newLogEntry);
            if (newLogEntry.Message != null)
            {
                listOfLogEntries.Add(newLogEntry);
            }
            return listOfLogEntries;
        }

        private void FileParserHelper(ref bool isCollectingNewStackTraceBlock, IEnumerable<string> logFile, List<LogEntry> listOfLogEntries, ref LogEntry newLogEntry)
        {
            foreach (var line in logFile)
            {
                if (string.IsNullOrEmpty(line)) continue;

                var logMatch = LogRegex.Match(line);
                if (logMatch.Success)
                {
                    if (newLogEntry.Message != null)
                    {
                        listOfLogEntries.Add(newLogEntry);
                    }

                    newLogEntry = ParseFirstLogLine(line);
                    isCollectingNewStackTraceBlock = true;
                    continue;
                }

                var stackMatch = StackTraceRegex.Match(line);
                if (isCollectingNewStackTraceBlock && stackMatch.Success)
                {
                    newLogEntry.AppendStackTraceLine(stackMatch.Groups["line"].Value);
                }
                else
                {
                    throw new InvalidOperationException($"Line format is invalid and does not match either log entry or stack trace patterns: {line}");
                }
            }
        }

        private LogEntry ParseFirstLogLine(string line)
        {
            var match = LogRegex.Match(line);
            if (!match.Success)
                throw new ArgumentException("Line format is invalid", nameof(line));

            var logEntry = CreateLogEntryHelper(match);

            return logEntry;

        }

        private void StackTraceParseLine(LogEntry logEntry, string line)
        {
            var match = StackTraceRegex.Match(line);
            if (!match.Success)
                throw new ArgumentException("StackTrace line not recognized by regex");

            var stackTraceMessage = match.Groups["line"].Value;

            logEntry.AppendStackTraceLine(stackTraceMessage);
        }

        private LogEntry CreateLogEntryHelper(Match match)
        {
            var timestamp = DateTime.Parse(match.Groups["timestamp"].Value);
            var level = match.Groups["level"].Value;
            var source = match.Groups["source"].Value;
            var logMessageFromFile = match.Groups["message"].Value;

            if (String.IsNullOrEmpty(timestamp.ToString()))
                throw new InvalidOperationException("Timestamp is null or empty!");

            if (level is null)
                throw new InvalidOperationException("Level is null or empty!");

            if (source is null)
                throw new InvalidOperationException("Source is null or empty!");

            if (logMessageFromFile is null)
                throw new InvalidOperationException("Source is null or empty!");


            var messageResult = LogMessage.Create(logMessageFromFile);
            if (messageResult == null || !messageResult.IsSuccess || messageResult.Value == null)
                throw new InvalidOperationException(messageResult?.Error ?? "LogMessage creation failed");
            var message = messageResult.Value;

            if (message is null)
                throw new InvalidOperationException("LogEntry is null or empty!");

            LogLevelEnum parsedEnumLevel = (LogLevelEnum)Enum.Parse(typeof(LogLevelEnum), level, true);

            if (String.IsNullOrEmpty(parsedEnumLevel.ToString()))
                throw new InvalidOperationException("Parsed enum is null or empty!");

            var logEntryResult = LogEntry.Create(message, string.Empty, source, parsedEnumLevel);
            if (logEntryResult == null || !logEntryResult.IsSuccess || logEntryResult.Value == null)
                throw new InvalidOperationException(logEntryResult?.Error ?? "LogEntry is null or empty!");

            return logEntryResult.Value;
        }

    }
}
