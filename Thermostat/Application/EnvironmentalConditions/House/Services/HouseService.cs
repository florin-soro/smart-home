using System.Diagnostics;
using Thermostat.Application.EnvironmentalConditions.House.Services.Dto;
using Thermostat.Application.EnvironmentalConditions.House.Services.Interfaces;
using Thermostat.Domain.Common.Exceptions;
using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg;
using Thermostat.Domain.Domain.HouseAgg.Repositories;
using Thermostat.Domain.Domain.HouseAgg.Room;
using Thermostat.Domain.Domain.HouseAgg.Sensor;

namespace Thermostat.Application.EnvironmentalConditions.House.Services
{
    public class PersistHouseService : IPersistHouseService
    {
        IHousePersistRepository houseRepository;

        public PersistHouseService(IHousePersistRepository houseRepository)
        {
            this.houseRepository = houseRepository;
        }

        public async Task<Guid> AddNewHouseAsync(List<SensorDto> sensors, string name)
        {
            Guid houseGuid = Guid.NewGuid();
            await houseRepository.AddHouseAsync(new HouseRoot(houseGuid,name, sensors.Select(s =>
            {
                bool ok = Enum.TryParse(s.RoomType, out RoomType roomType);
                if (!ok)
                {
                    throw new DomainRuleValidationException($"Invalid room type: {s.RoomType}");
                }
                return new SensorEntity(Guid.NewGuid(), new SensorTypeVO(s.Type), new SensorUnitVO(s.Unit), new RoomVO(s.RoomName, s.RoomArea, roomType));
            }).ToList()));
            return houseGuid;
        }

        public async Task<Guid> AddSensorToHouseAsync(Guid houseId, SensorDto sensorDto)
        {
            if (!Debugger.IsAttached)
                Debugger.Launch();
            Guid sensorGuid = Guid.Empty;
            var sensor = new SensorEntity(sensorGuid, new SensorTypeVO(sensorDto.Type), new SensorUnitVO(sensorDto.Unit), new RoomVO(sensorDto.RoomName, sensorDto.RoomArea, Enum.Parse<RoomType>(sensorDto.RoomType)));

            var house = await houseRepository.GetHouseAsync(houseId);
            if (house is null)
            {
                throw new EntityNotFoundException($"House with id {houseId} not found.");
            }
            house.AddSensor(sensor); 
            var updatedHouse = await houseRepository.UpdateAsync(house);
            return updatedHouse.Sensors.Last().Id;
        }
    }
}
