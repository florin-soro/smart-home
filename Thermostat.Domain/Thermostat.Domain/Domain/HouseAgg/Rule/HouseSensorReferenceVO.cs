using static Thermostat.Domain.Shared.Guard;

namespace Thermostat.Domain.Domain.HouseAgg.Rule
{
    public record HouseSensorReferenceVO
    {
        public Guid SensorId { get; }

        public HouseSensorReferenceVO(Guid sensorId)
        {
            if (sensorId == Guid.Empty)
                throw new ArgumentException("SensorId cannot be empty.", nameof(sensorId));

            SensorId = GuidNotEmpty(sensorId, nameof(sensorId));
        }
    }


}
