using LogAnalyzer.Core.Common;
using LogAnalyzer.Core.Entities.Enums;
using LogAnalyzer.Core.Events;

namespace LogAnalyzer.Core.Entities.LogEntryAggregate

{
    public sealed class LogEntry : AggregateRoot
    {
        public DateTime TimeStamp { get; set; }
        public LogLevelEnum LogLevel { get; set; }
        public LogMessage? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Source { get; set; }

        public LogEntry() { }
        public LogEntry(DateTime timeStamp, LogLevelEnum logLevel, LogMessage message, string? stackTrace, string? source)
        {
            TimeStamp = timeStamp;
            LogLevel = logLevel;
            Message = message;
            StackTrace = stackTrace;
            Source = source;

        }

        public static Result<LogEntry> Create(
            LogMessage message,
            string? stackTrace,
            string source,
            LogLevelEnum logLevel)
        {
            DateTime timeStamp = DateTime.UtcNow;
            if (!Enum.IsDefined(typeof(LogLevelEnum), logLevel))
                return Result<LogEntry>.Failure("Inappropiate Log Level value");
            if (message is null)
                return Result<LogEntry>.Failure("Message is null or empty");
            if (source is null || String.IsNullOrEmpty(source))
                return Result<LogEntry>.Failure("Log Source is null or empty");


            var logEntry = new LogEntry(timeStamp, logLevel, message, stackTrace, source);
            logEntry.RaiseDomainEvent(new LogEntryCreatedEvent(logEntry.Id));
            return Result<LogEntry>.Success(logEntry);
        }

        public LogEntry AppendStackTraceLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new ArgumentException("Stack trace line cannot be empty");

            if (string.IsNullOrEmpty(StackTrace))
            {
                StackTrace = line;
            }
            else
            {
                StackTrace += Environment.NewLine + line;
            }

            return this;
        }
    }
}
