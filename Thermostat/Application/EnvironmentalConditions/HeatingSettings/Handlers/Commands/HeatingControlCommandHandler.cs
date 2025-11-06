using MediatR;
using Microsoft.Extensions.Logging;
using Thermostat.Application.EnvironmentalConditions.HeatingSettings.Handlers.Commands.Exceptions;
using Thermostat.Application.EnvironmentalConditions.HeatingSettings.Services.Interfaces;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Commands.HeatingControlCommand;
using Thermostat.Domain.Shared;

namespace Thermostat.Application.EnvironmentalConditions.HeatingSettings.Handlers.Commands
{
    public class HeatingControlCommandHandler : IRequestHandler<HeatingControlCommand>
    {
        private readonly IHeatingDeviceControllerService heatingDeviceController;
        private readonly ILogger<HeatingControlCommandHandler> logger;

        public HeatingControlCommandHandler(IHeatingDeviceControllerService heatingDeviceController, ILogger<HeatingControlCommandHandler> _logger)
        {
            this.heatingDeviceController = heatingDeviceController;
            logger = _logger;
        }

        public async Task Handle(HeatingControlCommand request, CancellationToken cancellationToken)
        {
            switch (request.HeatingAction)
            {
                case HeatingAction.Start:
                    logger.LogInformation("Handling start heating action...");
                    await heatingDeviceController.StartAsync(cancellationToken);
                    break;

                case HeatingAction.Stop:
                    logger.LogInformation("Handling stop heating action...");
                    await heatingDeviceController.StopAsync(cancellationToken);
                    break;

                case HeatingAction.None:
                default:
                    throw new InvalidHeatingCommandException($"Invalid or unsupported heating action: {request.HeatingAction}");
            }
        }
    }
}
