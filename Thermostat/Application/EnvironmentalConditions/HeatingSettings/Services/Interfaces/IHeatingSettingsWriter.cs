namespace Thermostat.Application.EnvironmentalConditions.HeatingSettings.Services.Interfaces
{
    public interface IHeatingSettingsWriter
    { 
        Task WriteHeatingSettingsAsync(DateTime timestamp, double temperatureHigh, double temperatureLow, double humidityAlertThreshold);
    }
}
