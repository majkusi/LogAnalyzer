using LogAnalyzer.Core.Common;
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

        public void ParseLogs(string logsFilePath)
        {
            if (!File.Exists(logsFilePath)) return;

            var lines = File.ReadLines(logsFilePath);

            foreach (var line in lines)
            {

            }

        }


        private Result<LogEntry> ParseFirstLogLine(string line)
        {
            var match = LogRegex.Match(line);
            if (!match.Success)
                return Result<LogEntry>.Failure("Line format is invalid");

            var logEntry = CreateLogEntryHelper(match);

            return logEntry;

        }

        private Result<LogEntry> StackTraceParseLine(LogEntry logEntry, string line)
        {
            var match = StackTraceRegex.Match(line);
            if (!match.Success)
                return Result<LogEntry>.Failure("StackTrace line not recognized by regex");

            var stackTraceMessage = match.Groups["line"].Value;

            return logEntry.AppendStackTraceLine(line);
        }

        private Result<LogEntry> CreateLogEntryHelper(Match match)
        {
            var timestamp = DateTime.Parse(match.Groups["timestamp"].Value);
            var level = match.Groups["level"].Value;
            var source = match.Groups["source"].Value;
            var logMessageFromFile = match.Groups["message"].Value;

            if (String.IsNullOrEmpty(timestamp.ToString()))
                return Result<LogEntry>.Failure("Timestamp is null or empty!");

            if (level is null)
                return Result<LogEntry>.Failure("Level is null or empty!");

            if (source is null)
                return Result<LogEntry>.Failure("Source is null or empty!");

            if (logMessageFromFile is null)
                return Result<LogEntry>.Failure("Source is null or empty!");

            var message = LogMessage.Create(logMessageFromFile).Value;

            if (message is null)
                return Result<LogEntry>.Failure("LogEntry is null or empty!");

            LogLevelEnum parsedEnumLevel = (LogLevelEnum)Enum.Parse(typeof(LogLevelEnum), level, true);

            if (String.IsNullOrEmpty(parsedEnumLevel.ToString()))
                return Result<LogEntry>.Failure("Parsed enum is null or empty!");


            var logEntry = LogEntry.Create(message, string.Empty, source, parsedEnumLevel).Value;
            if (logEntry != null)
                return Result<LogEntry>.Success(logEntry);
            else
                return Result<LogEntry>.Failure("LogEntry is null or empty!");
        }

    }
}
