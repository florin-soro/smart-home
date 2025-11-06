using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;
namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext
{
    public class RuleDefinitionEntity
    {
        public Guid Id { get; set; }
        public RuleDefinitionType Type { get;  set; }
        public Guid RuleEntityId { get; set; }
        public virtual ICollection<RuleParameterEntity> Parameters { get; set; } = new List<RuleParameterEntity>();
        public RuleDefinitionEntity() { }
    }

}
