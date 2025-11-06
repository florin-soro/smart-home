using Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext;

namespace Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition
{
    public class GreaterThanRuleDefinition : RuleDefinition<IHouseRuleContext>
    {
        public override RuleDefinitionType RuleDefinitionType =>RuleDefinitionType.GreaterThan;
        public double Threshold { get; }
        public GreaterThanRuleDefinition(double threshold)
        {
            Threshold = threshold;
        }

        public override async Task<bool> IsSatisfiedByAsync(IHouseRuleContext context)
        {
            var sensorValue = await context.GetLastValueStartingFromAsync(DateTime.Now);
            bool isInRange = sensorValue != null && sensorValue.Value > Threshold;
            return isInRange;
        }
        public override IReadOnlyDictionary<string, string> Parameters => new Dictionary<string, string>
        {
            { nameof(Threshold), Threshold.ToString("0.#") }
        };
    }
}
    
