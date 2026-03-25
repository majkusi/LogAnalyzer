using LogAnalyzer.Core.Common;

namespace LogAnalyzer.Core.Events
{
    public sealed record LogGroupCreatedEvent(Guid logGroupId) : IDomainEvent
    {
    }
}
