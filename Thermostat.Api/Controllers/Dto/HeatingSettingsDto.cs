namespace Thermostat.Api.Controllers.Dto
{
    public class HeatingSettingsDto
    {
        public DateTime Timestamp { get; set; }
        public double TemperatureHigh { get; set; }
        public double TemperatureLow { get; set; }
        public double HumidityAlertThreshold { get; set; }
    }
}