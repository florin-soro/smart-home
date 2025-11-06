using Thermostat.Application.Interactors.RequetsDto;

namespace Thermostat.Application.EnvironmentalConditions.House.Services.Interfaces
{
    public interface ISensorMeasurementService
    {
        Task<InsertSensorMeasurementRequest?> GetPreviousMeasurementsAsync(Guid houawId, Guid aensorId, DateTime timesptamp);
        Task<List<InsertSensorMeasurementRequest>> GetMeasurementsAsync(Guid houseId, Guid sensorId, DateTime start, DateTime end);
        Task InsertMeasurementAsync(InsertSensorMeasurementRequest dto);
        Task<InsertSensorMeasurementRequest?> GetPreviousMeasurementStartingFromAsync(Guid houseId, Guid sensorId, DateTime timestamp);
    }
}
