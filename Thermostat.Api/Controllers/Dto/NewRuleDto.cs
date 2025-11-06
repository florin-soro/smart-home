using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;

namespace Thermostat.Application.EnvironmentalConditions.Rule.Controllers
{
        public class NewRuleDto
        {
            public string Name { get;  set; }
            public Guid HouseId { get;  set; }
            public Guid SensorId { get;  set; }
            public RuleDefinitionType RuleName { get;  set; }
            public Dictionary<string, string> RuleParameters { get;  set; }
            public string Action { get;  set; }
            public Dictionary<string, string> ActionParameters { get;  set; }
        }
}