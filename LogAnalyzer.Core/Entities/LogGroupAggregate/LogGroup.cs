using LogAnalyzer.Core.Common;
using LogAnalyzer.Core.Entities.LogEntryAggregate;

namespace LogAnalyzer.Core.Entities.LogGroupAggregate
{
    public sealed class LogGroup : AggregateRoot
    {
        public LogMessage? message { get; set; }
        public int count { get; set; }
        public DateTime firstOccurence { get; set; }
        public DateTime lastOccurence { get; set; }
        public IEnumerable<LogEntry> logs { get; set; } = Enumerable.Empty<LogEntry>();

        public LogGroup() { }
        public LogGroup(LogMessage? message, int count, DateTime firstOccurence, DateTime lastOccurence, IEnumerable<LogEntry> logs)
        {
            this.message = message;
            this.count = count;
            this.firstOccurence = firstOccurence;
            this.lastOccurence = lastOccurence;
            this.logs = logs;
        }

        public void AddLog(LogEntry log)
        {
            if (log is null)
                throw new ArgumentNullException(nameof(log));
            if (log.Message != message)
                throw new ArgumentException("Log message does not match LogGroup message!");
            lastOccurence = DateTime.Now;
            count++;
            logs.Append(log);
        }
    }
}
