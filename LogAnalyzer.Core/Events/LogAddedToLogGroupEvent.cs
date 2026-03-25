using LogAnalyzer.Core.Common;

namespace LogAnalyzer.Core.Events
{
    public sealed record LogAddedToLogGroupEvent(Guid logId, Guid logGroupId) : IDomainEvent
    {
    }
}
