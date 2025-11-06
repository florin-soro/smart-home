using MediatR;

namespace Thermostat.Domain.Domain.HeatingSettingsAgg.Commands.InsertHeatingSettingsCommand
{
    public record InsertHeatingSettingsCommand : IRequest
    {
        public HeatingSettingsRoot HeatingSettings { get; init; }
    }
}
