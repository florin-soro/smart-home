using Thermostat.Domain.Domain.HouseAgg.Rule;

namespace Thermostat.Application.EnvironmentalConditions.House.Actions
{
    public interface IActionExecutor<in TAction> : IActionExecutor where TAction : ActionDefinitionVO
    {
        public Task ExecuteAsync(TAction action);
    }

    public interface IActionExecutor
    {
        Task ExecuteAsync(ActionDefinitionVO action);
    }
}
