using LogAnalyzer.Core.Common;
using LogAnalyzer.Core.Entities.Enums;

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
    }
}
