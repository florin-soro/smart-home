using Thermostat.Domain.Common;

namespace Thermostat.Domain.Domain.Messages.Events
{
    public record DomainEvent<T>(T Entity, DateTime OccurredOn) : IDomainEvent where T : class;
}
