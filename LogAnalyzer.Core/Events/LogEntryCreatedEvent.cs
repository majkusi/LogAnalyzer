using LogAnalyzer.Core.Common;

namespace LogAnalyzer.Core.Events
{
    public sealed record LogEntryCreatedEvent(Guid logEntryId) : IDomainEvent
    {
    }
}
