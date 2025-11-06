using Thermostat.Domain.Domain.House.Sensor;

namespace Thermostat.Domain.Domain.Messages.Events
{
    public record MeasurementCreatedEvent(Guid HouseId, Guid SensorId,SensorMeasurementVO SensorMeasurementVO, DateTime OccurredOn) : DomainEvent<SensorMeasurementVO>(SensorMeasurementVO, OccurredOn);
}