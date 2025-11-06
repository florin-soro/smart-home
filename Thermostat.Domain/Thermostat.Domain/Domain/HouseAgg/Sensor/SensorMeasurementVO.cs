namespace Thermostat.Domain.Domain.House.Sensor
{
    public sealed record SensorMeasurementVO(DateTime Timestamp, double Value)
    {
        public bool Equals(SensorMeasurementVO? other)
        {
            return other is not null && Timestamp == other.Timestamp;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Timestamp, Value);
        }
    }
}
