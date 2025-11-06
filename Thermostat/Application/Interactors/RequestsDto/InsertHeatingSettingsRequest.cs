namespace Thermostat.Application.Interactors.RequetsDto
{
    public class InsertHeatingSettingsRequest
    {
        public DateTime Timestamp { get; set; }
        public double TemperatureHigh { get; set; }
        public double TemperatureLow { get; set; }
        public double HumidityAlertThreshold { get; set; }

        public InsertHeatingSettingsRequest() { }

        public InsertHeatingSettingsRequest(DateTime timestamp, double temperatureHigh, double temperatureLow, double humidityAlertThreshold)
        {
            Timestamp = timestamp;
            TemperatureHigh = temperatureHigh;
            TemperatureLow = temperatureLow;
            HumidityAlertThreshold = humidityAlertThreshold;
        }
    }
}
