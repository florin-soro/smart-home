using Thermostat.Domain.Common;
using static Thermostat.Domain.Shared.Guard;

namespace Thermostat.Domain.Domain.HeatingSettingsAgg
{
    public class HeatingSettingsRoot : EntityBase
    {
        public HeatingSettingsRoot(DateTime timestamp, TemperatureRangeVO temperatureRange, double humidityAlertThreshold):base()
        {
            Timestamp = timestamp;
            TemperatureRange = temperatureRange;
            HumidityAlertThreshold = AssignValidHumidityThreshold(humidityAlertThreshold);
        }

        private double AssignValidHumidityThreshold(double humidityAlertThreshold)
        {
            return Check(humidityAlertThreshold, h => h >= 0 && h<= 100, $"{nameof(humidityAlertThreshold)} should be in the range [0, 100]");
        }

        public HeatingSettingsRoot(Guid id, DateTime timestamp, TemperatureRangeVO temperatureRange, double humidityAlertThreshold) : base(id)
        {
            Timestamp = timestamp;
            TemperatureRange = temperatureRange;
            HumidityAlertThreshold = AssignValidHumidityThreshold(humidityAlertThreshold);
        }
        public DateTime Timestamp { get; }
        public TemperatureRangeVO TemperatureRange { get; }
        public double HumidityAlertThreshold { get; }
    }
}
