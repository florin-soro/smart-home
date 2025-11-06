using Thermostat.Application.EnvironmentalConditions.HeatingSettings.Services.Interfaces;

namespace Thermostat.DataAccessLayer.Shared.ExternalCommands
{
    public class HttpHeatingDeviceController : IHeatingDeviceControllerService
    {
        private readonly HttpClient _httpClient;

        public HttpHeatingDeviceController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task StartAsync(CancellationToken ct)
            => await _httpClient.GetAsync("http:/<...>/on", ct);

        public async Task StopAsync(CancellationToken ct)
            => await _httpClient.GetAsync("http:/<...>/off", ct);
    }
}
