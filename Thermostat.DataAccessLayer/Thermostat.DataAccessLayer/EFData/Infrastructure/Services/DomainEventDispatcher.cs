using MediatR;
using Thermostat.Application.Common.Notification;
using Thermostat.Domain.Common;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure.Services
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchAndClearEventsAsync(IEnumerable<AggregateRoot> entitiesWithEvents)
        {
            foreach (var entity in entitiesWithEvents)
            {
                var domainEvents = entity.DomainEvents.ToArray();
                entity.ClearEvents();

                foreach (var domainEvent in domainEvents)
                {
                    var domainEventType = domainEvent.GetType();
                    var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEventType);
                    var notification = (INotification)Activator.CreateInstance(notificationType, domainEvent)!;

                    await _mediator.Publish(notification);

                }

            }
        }
    }
}
