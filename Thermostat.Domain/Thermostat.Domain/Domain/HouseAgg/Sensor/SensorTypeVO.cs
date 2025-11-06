using static Thermostat.Domain.Shared.Guard;

namespace Thermostat.Domain.Domain.House.Sensor
{
    public record SensorTypeVO
    {
        public string TypeName { get; }

        public SensorTypeVO(string type)
        {
            TypeName = StringNotNullOrEmpty(type, nameof(type));
        }
    }
}
