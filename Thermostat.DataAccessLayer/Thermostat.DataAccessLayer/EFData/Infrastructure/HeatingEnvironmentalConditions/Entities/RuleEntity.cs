using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext;
namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities
{
    public class RuleEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SensorId { get; set; }
        public Guid HouseRootId { get; set; }
        public virtual RuleDefinitionEntity RuleDefinition { get; set; }
        public virtual ActionDefinitionEntity ActionDefinition { get; set; }
        public bool Enabled { get; set; }
    }
}
