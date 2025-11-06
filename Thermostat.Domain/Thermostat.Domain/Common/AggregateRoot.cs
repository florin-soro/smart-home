namespace Thermostat.Domain.Common
{
    public abstract class AggregateRoot : EntityBase
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;
        protected AggregateRoot() : base()
        { }
        protected AggregateRoot(Guid id) : base(id)
        { }
        protected virtual void AddDomainEvent(IDomainEvent newEvent)
        {
            _domainEvents.Add(newEvent);
        }

        public virtual void ClearEvents()
        {
            _domainEvents.Clear();
        }
    }
}
