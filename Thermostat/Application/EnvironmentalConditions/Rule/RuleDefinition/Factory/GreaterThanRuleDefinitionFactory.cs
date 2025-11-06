using Thermostat.Application.Common.Exceptions;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;

namespace Thermostat.Application.EnvironmentalConditions.Rule.RuleDefinition.Factory
{
    public class GreaterThanRuleDefinitionFactory : IRuleDefinitionFactory
    {
        public IRuleDefinitionVO Create(Dictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("Threshold", out var thresholdObj))
                throw new InvalidlParameterException("Missing Threshold parameters.");

            var threshold = Convert.ToDouble(thresholdObj.ToString());

            return new GreaterThanRuleDefinition(threshold);
        }
    }

}
