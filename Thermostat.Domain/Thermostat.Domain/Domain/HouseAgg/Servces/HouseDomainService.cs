using Thermostat.Domain.Domain.HouseAgg.Repositories;

namespace Thermostat.Domain.Domain.HouseAgg.Servces
{
    public class HouseDomainService : IHouseDomainService
    {
        private readonly IHousePersistRepository houseRepository;


        public HouseDomainService(IHousePersistRepository houseRepository)
        {
            this.houseRepository = houseRepository;
        }

        public async Task<bool> ExistsAsync(Guid houseId, Guid sensorId)
        {
            var house = await houseRepository.GetHouseAsync(houseId);
            return house.ContainSensor(sensorId);
        }
    }
}
