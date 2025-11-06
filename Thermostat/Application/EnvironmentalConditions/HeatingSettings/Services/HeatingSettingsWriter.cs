using MediatR;
using Thermostat.Application.EnvironmentalConditions.HeatingSettings.Services.Interfaces;
using Thermostat.Domain.Domain.HeatingSettingsAgg;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Commands.InsertHeatingSettingsCommand;

namespace Thermostat.Application.EnvironmentalConditions.HeatingSettings.Services
{
    public class HeatingSettingsWriter : IHeatingSettingsWriter
    {
        private readonly IMediator mediator;

        public HeatingSettingsWriter(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task WriteHeatingSettingsAsync(DateTime timestamp, double temperatureHigh, double temperatureLow, double humidityAlertThreshold)
        {
            await mediator.Send(new InsertHeatingSettingsCommand
            {
                HeatingSettings = new HeatingSettingsRoot(timestamp, new TemperatureRangeVO(temperatureLow, temperatureHigh), humidityAlertThreshold)
            });
        }
    }
}
