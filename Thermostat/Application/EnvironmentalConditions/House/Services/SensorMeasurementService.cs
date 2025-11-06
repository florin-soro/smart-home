using Thermostat.Application.EnvironmentalConditions.House.Services.Interfaces;
using Thermostat.Application.Interactors.RequetsDto;
using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg.Exceptions;
using Thermostat.Domain.Domain.HouseAgg.Sensor.Services;

namespace Thermostat.Application.EnvironmentalConditions.HouseAgg.Services
{
    public class SensorMeasurementService:ISensorMeasurementService
    {
        private readonly ISensorMeasurementsDomainService sensorService;

        public SensorMeasurementService(ISensorMeasurementsDomainService sensorDomainService)
        {
            sensorService = sensorDomainService;
        }

        public async Task InsertMeasurementAsync(InsertSensorMeasurementRequest dto)
        {
            var lastMeasurement = await GetPreviousMeasurementStartingFromAsync(dto.HouseId, dto.SensorId, DateTime.Now);
            if (lastMeasurement == null || dto.Timestamp > lastMeasurement.Timestamp)
            {
                await sensorService.SaveMeasurementAsync(dto.HouseId, dto.SensorId, new SensorMeasurementVO(dto.Timestamp, dto.Value));
            }
            else
            {
                throw new MeasurementEarlierOrEqualToLastMeasurementException($"A measurement with the same timestamp or earlier {dto.Timestamp} already exists for sensor with Id {dto.SensorId} from house {dto.HouseId}");
            }
        }

        public async Task<List<InsertSensorMeasurementRequest>> GetMeasurementsAsync(Guid houseId, Guid sensorId, DateTime start, DateTime end)
        {
            var measuments = await sensorService.GetMeasurementsAsync(houseId, sensorId, start, end);
            return measuments.Select(x=>new InsertSensorMeasurementRequest
            {
                HouseId = houseId,
                SensorId = sensorId,
                Timestamp = x.Timestamp,
            }).ToList();
        }

        public async Task<InsertSensorMeasurementRequest?> GetPreviousMeasurementStartingFromAsync(Guid houseId, Guid sensorId, DateTime timestamp)
        {
            var lastMeasurement = await sensorService.GetPreviousMeasurementStartingFromAsync(houseId, sensorId, timestamp);
            if (lastMeasurement != null)
            {
                return new InsertSensorMeasurementRequest
                {
                    HouseId = houseId,
                    SensorId = sensorId,
                    Timestamp = lastMeasurement.Timestamp,
                    Value = lastMeasurement.Value
                };
            }
            return null;
        }

        public async Task<InsertSensorMeasurementRequest?> GetPreviousMeasurementsAsync(Guid houawId, Guid aensorId, DateTime timesptamp)
        {
            var lastMeasurement = await sensorService.GetPreviousMeasurementsAsync(houawId, aensorId,timesptamp);
            if (lastMeasurement != null)
            {
                return new InsertSensorMeasurementRequest
                {
                    HouseId = houawId,
                    SensorId = aensorId,
                    Timestamp = lastMeasurement.Timestamp,
                    Value = lastMeasurement.Value
                };
            }
            return null;
        }
    }
}
