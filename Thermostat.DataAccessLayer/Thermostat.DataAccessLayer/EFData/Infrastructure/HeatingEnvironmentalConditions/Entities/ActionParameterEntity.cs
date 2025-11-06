namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities
{
    public class ActionParameterEntity
    {
        public Guid Id { get;  set; }
        public string Name { get;  set; }
        public string Value { get;  set; }
        public string Type { get; set; }

        public Guid ActionDefinitionEntityId { get; set; }
        public virtual ActionDefinitionEntity ActionEntity { get; set; }
        public ActionParameterEntity()
        {
        }
        public ActionParameterEntity(string name, string value)
        {
            Name = name;
            Value = value;
            Type = value.GetType().Name;
        }
    }
}
