using Thermostat.Application.Interactors.RequetsDto;
using Thermostat.Domain.Domain.HouseAgg;
using Thermostat.Domain.Domain.HouseAgg.Sensor.Services;
namespace Thermostat.Application.Interactors
{
    public class GetHouseWithSensorMeasurementsInteractor
    {
        private readonly ISensorMeasurementsDomainService sensorService;

        public GetHouseWithSensorMeasurementsInteractor(ISensorMeasurementsDomainService sensorService)
        {
            this.sensorService = sensorService;
        }

        public async Task<HouseRoot> HandleAsync(GetHouseWithSensorMeasurementsRequest request)
        {
            var startDate = request.Start ?? DateTime.MinValue;
            var endDate = request.End ?? DateTime.MaxValue;
            return await sensorService.GetHouseWithSensorMeasurements(request.HouseId, startDate, endDate);
        }
    }
}
