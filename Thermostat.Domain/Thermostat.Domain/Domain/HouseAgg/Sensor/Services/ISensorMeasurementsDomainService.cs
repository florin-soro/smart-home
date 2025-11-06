using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg;

namespace Thermostat.Domain.Domain.HouseAgg.Sensor.Services
{
    public interface ISensorMeasurementsDomainService
    {
        Task SaveMeasurementAsync(Guid homeId, Guid sensorId, SensorMeasurementVO measurement);
        Task<SensorMeasurementVO> GetPreviousMeasurementsAsync(Guid houawId, Guid aensorId, DateTime timesptamp);
        Task<List<SensorMeasurementVO>> GetMeasurementsAsync(Guid homeId, Guid sensorId, DateTime start, DateTime end);
        Task<SensorMeasurementVO> GetPreviousMeasurementStartingFromAsync(Guid houseId, Guid sensorId, DateTime timestamp);
        Task<HouseRoot> GetHouseWithSensorMeasurements(Guid houseId, DateTime start, DateTime end);
    }
}