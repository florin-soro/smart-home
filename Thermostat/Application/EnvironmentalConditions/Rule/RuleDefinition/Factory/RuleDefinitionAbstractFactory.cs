using Thermostat.Application.EnvironmentalConditions.Rule.Exceptions;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;

namespace Thermostat.Application.EnvironmentalConditions.Rule.RuleDefinition.Factory
{
    public class RuleDefinitionAbstractFactory
    {
        private readonly Dictionary<RuleDefinitionType, IRuleDefinitionFactory> _factories;

        public RuleDefinitionAbstractFactory()
        {
            _factories = new Dictionary<RuleDefinitionType, IRuleDefinitionFactory>
            {
                { RuleDefinitionType.Between, new BetweenRuleDefinitionFactory() },
                { RuleDefinitionType.LessThan, new LessThanRuleDefinitionFactory() },
                { RuleDefinitionType.GreaterThan, new GreaterThanRuleDefinitionFactory() }
                // Add more factories here
            };
        }

        public IRuleDefinitionVO Create(RuleDefinitionType type, Dictionary<string, string> parameters)
        {
            if (!_factories.TryGetValue(type, out var factory))
                throw new RuleNotSupportedException($"Rule type '{type}' is not supported.");

            return factory.Create(parameters);
        }
    }
}
