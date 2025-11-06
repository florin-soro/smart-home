using Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext;

namespace Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition
{
    public class LessThanRuleDefinition : RuleDefinition<IHouseRuleContext>
    {
        public override RuleDefinitionType RuleDefinitionType => RuleDefinitionType.LessThan;
        public double Threshold { get; }
        public LessThanRuleDefinition(double threshold)
        {
            Threshold = threshold;
        }

        public override async Task<bool> IsSatisfiedByAsync(IHouseRuleContext context)
        {
            var sensorValue = await context.GetLastValueStartingFromAsync(DateTime.Now);
            bool isInRange = sensorValue != null && Threshold > sensorValue.Value;
            return isInRange;
        }
        public override Dictionary<string, string> Parameters => new Dictionary<string, string>
        {
            { nameof(Threshold), Threshold.ToString("0.#") }
        };
    }
}
    
