using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Thermostat.Application.Common.Notification;
using Thermostat.Domain.Common;

namespace Thermostat.Application.Common.Handlers
{
    public class DomainEventNotificationHandler<TDomainEvent> : INotificationHandler<DomainEventNotification<TDomainEvent>>
        where TDomainEvent : IDomainEvent
    {
        private readonly ILogger<DomainEventNotificationHandler<TDomainEvent>> _logger;

        public DomainEventNotificationHandler(ILogger<DomainEventNotificationHandler<TDomainEvent>> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<TDomainEvent> notification, CancellationToken cancellationToken)
        {
            var eventType = notification.DomainEvent.GetType().Name;
            var entity = notification.DomainEvent;
            var entityJson = JsonSerializer.Serialize(entity);

            _logger.LogInformation(
                "New Domain Event Created: A new {eventType} was created with content: {EntityJson}",
                eventType,
                entityJson 
            );

            return Task.CompletedTask;
        }
    }
}