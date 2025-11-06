using static Thermostat.Domain.Shared.Guard;

namespace Thermostat.Domain.Domain.HeatingSettingsAgg
{
    public class TemperatureRangeVO
    {
        public double MinTemperature { get; }
        public double MaxTemperature { get; }
        public TemperatureRangeVO(double minTemperature, double maxTemperature)
        {
            IsMet(minTemperature, maxTemperature, (low, high) => low < high, $"{nameof(minTemperature)} must be less than {nameof(maxTemperature)}");
            MinTemperature = minTemperature;
            MaxTemperature = maxTemperature;
        }
    }
}