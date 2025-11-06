using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;

namespace Thermostat.Application.EnvironmentalConditions.Rule.RuleDefinition.Factory
{
    public interface IRuleDefinitionFactory
    {
        IRuleDefinitionVO Create(Dictionary<string, string> parameters);
    }
}
