using MediatR;
using Thermostat.Domain.Shared;

namespace Thermostat.Domain.Domain.HeatingSettingsAgg.Commands.HeatingControlCommand
{
    public record HeatingControlCommand : IRequest
    {
        public HeatingAction HeatingAction { get; set; }
    }
}
