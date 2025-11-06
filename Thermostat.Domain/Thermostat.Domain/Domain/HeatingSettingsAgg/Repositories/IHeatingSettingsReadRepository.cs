namespace Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories
{
    public interface IHeatingSettingsReadRepository
    {
        Task<double> GetTemperatureHighAsync(DateTime at);
        Task<double> GetOutlierTemperatureThresholdValueAsync(DateTime at);
        Task<double> GetTemperatureLowAsync(DateTime at);
    }
}
