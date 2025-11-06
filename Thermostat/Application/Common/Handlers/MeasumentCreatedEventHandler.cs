using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Thermostat.Application.Common.Notification;
using Thermostat.Domain.Domain.Messages.Events;

namespace Thermostat.Application.Common.Handlers
{
    public class MeasumentCreatedEventHandler : INotificationHandler<DomainEventNotification<MeasurementCreatedEvent>>
    {
        private readonly ILogger<MeasumentCreatedEventHandler> logger;

        public MeasumentCreatedEventHandler(ILogger<MeasumentCreatedEventHandler> logger)
        {
            this.logger = logger;
        }
        public Task Handle(DomainEventNotification<MeasurementCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var entityType = nameof(MeasurementCreatedEvent);
            var entity = notification.DomainEvent.Entity;

            // Serialize the entity to capture its initial state
            var entityData = JsonSerializer.Serialize(entity);

            logger.LogInformation($"Measurement Created: A new {entityType} occured on {notification.DomainEvent.OccurredOn} havving sensorId {notification.DomainEvent.SensorId} this structure {entityData}");

            return Task.CompletedTask;
        }
    }
}
