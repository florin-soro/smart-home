namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities
{
    public class RuleParameterEntity
    {
        public Guid Id { get;  set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }

        public Guid RuleDefinitionEntityId { get; set; }
        public RuleParameterEntity()
        {
            
        }
        public RuleParameterEntity(string name, string value)
        {
            Name = name;
            Value = value;
            Type = value.GetType().Name;
        }
    }
}
