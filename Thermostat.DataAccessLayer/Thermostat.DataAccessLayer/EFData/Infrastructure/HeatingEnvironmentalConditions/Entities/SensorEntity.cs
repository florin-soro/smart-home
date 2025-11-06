namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities
{
    public class SensorEntity
    {
        public SensorEntity() { }
        public Guid Id { get; set; }
        public Guid HouseRootId { get; set; }
        public string Type { get; set; }
        public string Unit { get; set; }
        public string RoomName { get; set; }
        public double RoomArea { get; set; }
        public string RoomType { get; set; }

        public virtual List<SensorMeasurementEntity> Measurements { get; set; } = new();
    }
}