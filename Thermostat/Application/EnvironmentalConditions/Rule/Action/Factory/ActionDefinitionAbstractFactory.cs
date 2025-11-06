using Thermostat.Domain.Domain.HouseAgg.Rule;

namespace Thermostat.DataAccessLayer.Data.Infrastructure.HeatingEnvironmentalConditions.Actions.Factories
{
    public class ActionDefinitionAbstractFactory
    {
        public ActionDefinitionVO Create(string type, Dictionary<string, string> parameters)
        {
            return type switch
            {
                "HeatingControl" => new HeatingControlActionDefinitionFactory().Create(parameters),
                _ => throw new NotSupportedException($"Action type '{type}' is not supported.")
            };
        }
    }
}
