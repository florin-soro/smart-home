using Thermostat.Application.EnvironmentalConditions.House.Services.Dto;
using Thermostat.Application.EnvironmentalConditions.House.Services.Interfaces;
using Thermostat.Application.Interactors.RequetsDto;
namespace Thermostat.Application.Interactors
{
    public class AddSensorInteractor
    {
        private readonly IPersistHouseService persistHouseService;

        public AddSensorInteractor(IPersistHouseService persistHouseService)
        {
            this.persistHouseService = persistHouseService;
        }

        public async Task<Guid> HandleAsync(CreateSensorRequest sensorRequest)
        {
            var sensor = new SensorDto
            {
                RoomArea = sensorRequest.RoomArea,
                RoomName = sensorRequest.RoomName,
                RoomType = sensorRequest.RoomType,
                Type = sensorRequest.Type,
                Unit = sensorRequest.Unit
            };

            return await persistHouseService.AddSensorToHouseAsync(sensorRequest.HouseId, sensor);
        }
    }
}
