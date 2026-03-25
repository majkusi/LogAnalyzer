using LogAnalyzer.Core.Common;

namespace LogAnalyzer.Core.Entities.LogEntryAggregate
{
    public sealed class LogMessage : ValueObject
    {
        private const int MESSAGE_SANITY_LIMIT = 1000;

        public string Message { get; private set; }

        private LogMessage(string message)
        {
            Message = message;
        }

        public static Result<LogMessage> Create(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                return Result<LogMessage>.Failure("message is empty");
            }
            if (message.Length > MESSAGE_SANITY_LIMIT)
            {
                return Result<LogMessage>.Failure("message is too long");
            }

            return Result<LogMessage>.Success(new LogMessage(message));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Message;
        }
    }
}
