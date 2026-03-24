namespace LogAnalyzer.Core.Common
{
    public abstract class AggregateRoot : BaseAuditableEntity
    {
        public Guid Id { get; set; }
        public readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        public void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    }
}
