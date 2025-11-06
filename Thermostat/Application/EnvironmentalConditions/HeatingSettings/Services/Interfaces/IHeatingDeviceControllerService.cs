namespace Thermostat.Application.EnvironmentalConditions.HeatingSettings.Services.Interfaces
{
    public interface IHeatingDeviceControllerService
    {
        Task StartAsync(CancellationToken ct);
        Task StopAsync(CancellationToken ct);
    }
}
