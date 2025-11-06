using Thermostat.Domain.Common;

namespace Thermostat.Domain.Domain.Messages.Events
{
    /// <summary>
    /// A generic event published when any new entity is created.
    /// It inherits from the base DomainEvent to hold a reference to the entity.
    /// </summary>
    /// <typeparam name="T">The type of the entity that was created.</typeparam>
    public record AggregateOperationDoneEvent(Guid aggregateId,string actionName, EntityBase Entity, DateTime OccurredOn)
        : DomainEvent<EntityBase>(Entity, OccurredOn);

    public record AggregateOperationFailedEvent(Guid aggregateId,string actionName, string Payload,string message, DateTime OccurredOn) : DomainEvent<string>(Payload, OccurredOn);

}