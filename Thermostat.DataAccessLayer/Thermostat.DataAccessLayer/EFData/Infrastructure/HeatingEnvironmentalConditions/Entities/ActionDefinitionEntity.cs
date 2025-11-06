namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities
{
    public class ActionDefinitionEntity
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid RuleEntityId { get; set; }

        public virtual ICollection<ActionParameterEntity> Parameters { get; set; } = new List<ActionParameterEntity>();

        public ActionDefinitionEntity() { } 
    }
}
