using MediatR;
using Thermostat.Application.EnvironmentalConditions.Rule.Action;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Commands.HeatingControlCommand;
using Thermostat.Domain.Shared;

namespace Thermostat.DataAccessLayer.Shared.Actions.HeatingControl
{
    public class HeatingControlActionExecutor : ActionExecutorBase<HeatingControlAction>
    {
        private readonly IMediator mediator;

        public HeatingControlActionExecutor(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public override async Task ExecuteAsync(HeatingControlAction heatingAction)
        {
            if (heatingAction == null)
                throw new InvalidOperationException($"Invalid action type: {heatingAction.GetType().Name}. Expected HeatingControlAction.");
            HeatingControlCommand commandToExecute = new HeatingControlCommand { HeatingAction = Enum.Parse<HeatingAction>(heatingAction.Command)};

            if (commandToExecute == null)
                throw new InvalidOperationException($"No command found for action: {heatingAction.Command}");

            await mediator.Send(commandToExecute);
        }
    }
}
