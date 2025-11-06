using MediatR;

namespace Thermostat.Domain.Common
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAndClearEventsAsync(IEnumerable<AggregateRoot> aggregates);
    }

}
