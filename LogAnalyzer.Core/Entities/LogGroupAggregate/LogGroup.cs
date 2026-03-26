using LogAnalyzer.Core.Common;
using LogAnalyzer.Core.Entities.LogEntryAggregate;
using LogAnalyzer.Core.Events;

namespace LogAnalyzer.Core.Entities.LogGroupAggregate
{
    public sealed class LogGroup : AggregateRoot
    {
        public LogMessage message { get; private set; }
        public int count { get; private set; }
        public DateTime firstOccurence { get; private set; }
        public DateTime lastOccurence { get; private set; }
        public IReadOnlyCollection<Guid> Logs => _logs.AsReadOnly();
        private List<Guid> _logs { get; set; }

        private LogGroup() { }
        private LogGroup(LogMessage message, int count, DateTime firstOccurence, DateTime lastOccurence, List<Guid> logs)
        {
            this.message = message ?? throw new ArgumentNullException(nameof(message));
            this.count = count;
            this.firstOccurence = firstOccurence;
            this.lastOccurence = lastOccurence;
            this._logs = logs ?? new List<Guid>();
        }

        public static Result<LogGroup> Create(
            LogMessage message,
            Guid firstLogId,
            DateTime timestamp)
        {
            if (message is null)
                return Result<LogGroup>.Failure("Message is null!");
            if (firstLogId == Guid.Empty)
                return Result<LogGroup>.Failure("FirstLogId is null or empty");
            if (String.IsNullOrEmpty(timestamp.ToString()))
                return Result<LogGroup>.Failure("Timestamp is null or empty!");

            List<Guid> firstLogList = new List<Guid>();
            firstLogList.Add(firstLogId);
            var logGroup = new LogGroup(message, 1, timestamp, timestamp, firstLogList);

            logGroup.RaiseDomainEvent(new LogGroupCreatedEvent(logGroup.Id));

            return Result<LogGroup>.Success(logGroup);
        }

        public void AddLog(Guid logId, LogMessage message, DateTime timestamp)
        {
            if (logId == Guid.Empty)
                throw new ArgumentNullException(nameof(logId));
            if (message.Message != this.message.Message)
                throw new ArgumentException("Log message does not match LogGroup message!");

            this.lastOccurence = timestamp;
            this.count++;

            this.RaiseDomainEvent(new LogAddedToLogGroupEvent(logId, this.Id));
            this._logs.Add(logId);
        }
    }
}
