using Thermostat.Domain.Domain.House.Repositories;
using Thermostat.Domain.Domain.House.Sensor;

namespace Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext
{
    public class HouseRuleContext : IHouseRuleContext
    {
        private readonly IHouseQueryRepository houseRepository;
        private readonly Guid houseId;
        private readonly Guid sensorId;

        public HouseRuleContext(IHouseQueryRepository houseRepository, Guid houseId, Guid sensorId)
        {
            this.houseRepository = houseRepository;
            this.houseId = houseId;
            this.sensorId = sensorId;
        }

        public string ContextName => nameof(HouseRuleContext);

        public async  Task<SensorMeasurementVO?> GetLastValueStartingFromAsync(DateTime timestamp)
        {
            return await houseRepository.GetLastMeasurementAsync(houseId, sensorId, timestamp);
        }

        public async Task<IEnumerable<SensorMeasurementVO>> GetSensorValuesAsync(DateTime start, DateTime end)
        {
            return await houseRepository.GetMeasurementsAsync(houseId, sensorId,start, end);
        }
    }
}
