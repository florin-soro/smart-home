using Thermostat.Application.EnvironmentalConditions.Rule.Action;
using Thermostat.Domain.Domain.HouseAgg.Rule;

namespace Thermostat.DataAccessLayer.Data.Infrastructure.HeatingEnvironmentalConditions.Actions.Factories
{
    public class HeatingControlActionDefinitionFactory : IActionDefinitionFactory<ActionDefinitionVO>
    {
        public ActionDefinitionVO Create(Dictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("Command", out var cmdObj))
                throw new ArgumentException("Missing 'name' parameter.");

            var command = cmdObj.ToString();
            return new HeatingControlAction(command!);
        }
    }
}
