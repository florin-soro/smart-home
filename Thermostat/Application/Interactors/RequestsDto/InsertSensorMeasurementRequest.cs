namespace Thermostat.Application.Interactors.RequetsDto
{
    public class InsertSensorMeasurementRequest
    {
        public Guid HouseId { get; set; }
        public Guid SensorId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
    }
}
