namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities
{
    public class HouseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual List<SensorEntity> Sensors { get; set; } = new();
        public virtual List<RuleEntity> Rules { get; set; } = new();
    }
}
