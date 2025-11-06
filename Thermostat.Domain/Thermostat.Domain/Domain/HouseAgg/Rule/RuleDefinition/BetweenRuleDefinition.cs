using Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext;

namespace Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition
{
    public class BetweenRuleDefinition : RuleDefinition<IHouseRuleContext>
    {
        public override RuleDefinitionType RuleDefinitionType => RuleDefinitionType.Between;
        public double Min { get; }
        public double Max { get; }
        public BetweenRuleDefinition(double min, double max)
        {
            if (min > max)
                throw new ArgumentException("Min cannot be greater than Max.");

            Min = min;
            Max = max;
        }

        public override async Task<bool> IsSatisfiedByAsync(IHouseRuleContext context)
        {
            var sensorValue = await context.GetLastValueStartingFromAsync(DateTime.Now);
            bool isInRange = sensorValue != null && sensorValue.Value >= Min && sensorValue.Value <= Max;
            return isInRange;
        }
        public override Dictionary<string, string> Parameters => new Dictionary<string, string>
            {
                { "Min", Min.ToString("0.#") },
                { "Max", Max.ToString("0.#") }
            };
    }
}
    
