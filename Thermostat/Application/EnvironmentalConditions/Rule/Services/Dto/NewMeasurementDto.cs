namespace Thermostat.Application.EnvironmentalConditions.Rule.Services.Dto
{
    public record NewMeasurementDto(DateTime timestamp, double value)
    {
        public DateTime Timestamp { get; } = timestamp;
        public double Value { get; } = value;
    }
}
