using Thermostat.Application.EnvironmentalConditions.House.Services.Dto;
using Thermostat.Application.EnvironmentalConditions.House.Services.Interfaces;
using Thermostat.Application.Interactors.RequetsDto;
namespace Thermostat.Application.Interactors
{
    public class CreateHouseInteractor
    {
        private readonly IPersistHouseService persistHouseService;

        public CreateHouseInteractor(IPersistHouseService persistHouseService)
        {
            this.persistHouseService = persistHouseService;
        }

        public async Task<Guid> HandleAsync(CreateHouseRequest command)
        {
            var sensors = command.Sensors.Select(x => new SensorDto
            {
                RoomArea = x.RoomArea,
                RoomName = x.RoomName,
                RoomType = x.RoomType,
                Type = x.Type,
                Unit = x.Unit
            }).ToList();

            return await persistHouseService.AddNewHouseAsync(sensors, command.Name);
        }
    }
}
