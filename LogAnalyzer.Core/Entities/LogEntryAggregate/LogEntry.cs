using LogAnalyzer.Core.Common;
using LogAnalyzer.Core.Entities.Enums;

namespace LogAnalyzer.Core.Entities.LogEntryAggregate

{
    public class LogEntry : AggregateRoot
    {
        public DateTime TimeStamp { get; set; }
        public LogLevelEnum LogLevel { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
        public string? Source { get; set; }

    }
}
