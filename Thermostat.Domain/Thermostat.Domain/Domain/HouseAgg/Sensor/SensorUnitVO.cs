using static Thermostat.Domain.Shared.Guard;

namespace Thermostat.Domain.Domain.HouseAgg.Sensor
{
    public record SensorUnitVO
    {
        public string UnitName { get; }

        public SensorUnitVO(string unitName)
        {
            UnitName = StringNotNullOrEmpty(unitName, nameof(unitName));
        }
    }
}
