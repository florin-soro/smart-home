using Thermostat.Application.EnvironmentalConditions.House.Services.Dto;

namespace Thermostat.Application.EnvironmentalConditions.House.Services.Interfaces
{
    public interface IPersistHouseService
    {
        Task<Guid> AddNewHouseAsync(List<SensorDto> sensors, string name);
        Task<Guid> AddSensorToHouseAsync(Guid houseId, SensorDto sensorDto);
    }
}
