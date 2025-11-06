using Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext;

namespace Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition
{
    public abstract class RuleDefinition<T> : IRuleDefinition<T> where T : IRuleContext
    {
        public abstract Task<bool> IsSatisfiedByAsync(T value);

        public abstract RuleDefinitionType RuleDefinitionType { get; }
        public abstract IReadOnlyDictionary<string, string> Parameters { get; }
    }
}
