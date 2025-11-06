using Thermostat.Application.Common.Exceptions;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;

namespace Thermostat.Application.EnvironmentalConditions.Rule.RuleDefinition.Factory
{
    public class BetweenRuleDefinitionFactory : IRuleDefinitionFactory
    {
        public IRuleDefinitionVO Create(Dictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("Min", out var minObj) || !parameters.TryGetValue("Max", out var maxObj))
                throw new InvalidlParameterException("Missing Min or Max parameters.");

            var min = Convert.ToDouble(minObj);
            var max = Convert.ToDouble(maxObj);

            return new BetweenRuleDefinition(min, max);
        }
    }

}
