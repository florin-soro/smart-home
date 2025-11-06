using Thermostat.Domain.Common;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;
using Thermostat.Domain.Domain.HouseAgg.Sensor;
using static Thermostat.Domain.Shared.Guard;

namespace Thermostat.Domain.Domain.HouseAgg.Rule
{
    public class RuleEntity : EntityBase
    {
        public IRuleDefinitionVO RuleDefinition { get; private set; }
        public RuleNameVO Name { get; private set; }
        public SensorEntity Sensor { get; private set; }
        public ActionDefinitionVO Action { get; private set; }
        public bool Enabled { get; private set; }

        public RuleEntity(SensorEntity sensor, RuleNameVO name, IRuleDefinitionVO ruleDefinition, ActionDefinitionVO action) : base()
        {
            Init(sensor, name, ruleDefinition, action);
        }

        private void Init(SensorEntity sensor, RuleNameVO name, IRuleDefinitionVO ruleDefinition, ActionDefinitionVO action, bool enabled = false)
        {
            Sensor = NotNull(sensor, nameof(sensor));
            Name = NotNull(name, nameof(name));
            RuleDefinition = NotNull(ruleDefinition, nameof(ruleDefinition));
            Action = NotNull(action, nameof(action));
            Enabled = enabled;
        }

        public RuleEntity(Guid id, SensorEntity sensor, RuleNameVO name, IRuleDefinitionVO ruleDefinition, ActionDefinitionVO action,bool enabled) : base(id)
        {
            Init(sensor, name, ruleDefinition, action, enabled);
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public async Task<bool> EvaluateAsync(IRuleContext context)
        {
            return Enabled && await RuleDefinition.IsSatisfiedByAsync(context);
        }
    }
}
