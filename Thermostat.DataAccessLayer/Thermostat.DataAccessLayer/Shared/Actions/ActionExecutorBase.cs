using Thermostat.Application.EnvironmentalConditions.House.Actions;
using Thermostat.Domain.Domain.HouseAgg.Rule;

namespace Thermostat.DataAccessLayer.Shared.Actions
{
    public abstract class ActionExecutorBase<T> : IActionExecutor<T> where T: ActionDefinitionVO
    {
        public abstract Task ExecuteAsync(T action);
        public async Task ExecuteAsync(ActionDefinitionVO action)
        {
            if (action is not T typedAction)
            {
                throw new InvalidOperationException($"Invalid action type: {action.GetType().Name}. Expected {typeof(T).Name}.");
            }
            await ExecuteAsync(typedAction);
        }
    }
}
