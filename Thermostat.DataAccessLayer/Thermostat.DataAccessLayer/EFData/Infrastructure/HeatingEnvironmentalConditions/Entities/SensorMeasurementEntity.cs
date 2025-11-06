namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities
{
    public class SensorMeasurementEntity
    {
        public Guid HouseRootId { get; set; }
        public Guid SensorEntityId { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual SensorEntity Sensor { get; set; }
    }
}
