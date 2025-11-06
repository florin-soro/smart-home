using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg;
using Thermostat.Domain.Domain.HouseAgg.Rule;

namespace Thermostat.Domain.Domain.House.Repositories
{
    public interface IHouseQueryRepository
    {
        Task<List<SensorMeasurementVO>> GetMeasurementsAsync(Guid houseId, Guid sensorId, DateTime start, DateTime end);
        Task<SensorMeasurementVO?> GetLastMeasurementAsync(Guid houseId, Guid sensorId, DateTime timestamp);
        Task<SensorMeasurementVO> GetPreviousMeasurementStartingFromAsync(Guid houseId, Guid sensorId, DateTime timestamp);
        Task<List<RuleEntity>> GetRulesAsync(Guid houseId, Guid sensorId, bool v);
        Task<HouseRoot> GetHouseWithSensorMeasurements(Guid houseId, DateTime start, DateTime end);
    }
}