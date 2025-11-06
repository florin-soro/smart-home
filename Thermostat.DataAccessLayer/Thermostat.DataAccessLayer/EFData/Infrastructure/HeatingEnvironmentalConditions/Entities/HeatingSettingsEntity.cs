namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities
{
    public class HeatingSettingsEntity
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double TempLow { get; set; }
        public double TempHigh { get; set; }
        public double HumidityAlertThreshold { get; set; }
    }
}
