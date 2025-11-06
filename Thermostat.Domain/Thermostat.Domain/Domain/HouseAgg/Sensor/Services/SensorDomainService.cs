using Thermostat.Domain.Domain.House.Repositories;
using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg.Exceptions;
using Thermostat.Domain.Domain.HouseAgg.Repositories;

namespace Thermostat.Domain.Domain.HouseAgg.Sensor.Services
{
    public class SensorMeasurementsDomainService : ISensorMeasurementsDomainService
    {
        IHouseQueryRepository _homeQueryRepository;
        IHousePersistRepository _housePersistRepository;

        public SensorMeasurementsDomainService(IHouseQueryRepository homeRepository, IHousePersistRepository housePersistRepository)
        {
            _homeQueryRepository = homeRepository;
            _housePersistRepository = housePersistRepository;
        }

        public async Task SaveMeasurementAsync(Guid homeId, Guid sensorId, SensorMeasurementVO measurement)
        {
            var home = await _housePersistRepository.GetHouseAsync(homeId);

            if (!home.AddMeasurementToSensor(sensorId, measurement))
            {
                throw new MeasurementEarlierOrEqualToLastMeasurementException($"A measurement with the same timestamp {measurement.Timestamp} already exists fro sensor with Id {sensorId} from home {homeId}");
            }

            _= await _housePersistRepository.UpdateAsync(home);
        }

        public async Task<SensorMeasurementVO> GetPreviousMeasurementsAsync(Guid houawId, Guid aensorId, DateTime timesptamp)
        {
            return await _homeQueryRepository.GetLastMeasurementAsync(houawId, aensorId, timesptamp);
        }

        public async Task<List<SensorMeasurementVO>> GetMeasurementsAsync(Guid homeId, Guid sensorId, DateTime start, DateTime end)
        {
            return await _homeQueryRepository.GetMeasurementsAsync(homeId, sensorId, start, end);
        }

        public async Task<SensorMeasurementVO> GetPreviousMeasurementStartingFromAsync(Guid houseId, Guid sensorId, DateTime timestamp)
        {
            return await _homeQueryRepository.GetPreviousMeasurementStartingFromAsync(houseId, sensorId, timestamp);   
        }

        public async Task<HouseRoot> GetHouseWithSensorMeasurements(Guid houseId, DateTime start, DateTime end)
        {
            return await _homeQueryRepository.GetHouseWithSensorMeasurements(houseId, start, end);
        }
    }
}
