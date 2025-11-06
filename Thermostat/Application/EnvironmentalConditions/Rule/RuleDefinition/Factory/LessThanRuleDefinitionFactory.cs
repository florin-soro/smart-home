using Thermostat.Application.Common.Exceptions;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;

namespace Thermostat.Application.EnvironmentalConditions.Rule.RuleDefinition.Factory
{
    public class LessThanRuleDefinitionFactory : IRuleDefinitionFactory
    {
        public IRuleDefinitionVO Create(Dictionary<string, string> parameters)
        {
            if (!parameters.TryGetValue("Threshold", out var thresholdObj))
                throw new InvalidlParameterException("Missing Min or Max parameters.");

            var threshold = Convert.ToDouble(thresholdObj);

            return new LessThanRuleDefinition(threshold);
        }
    }
}
