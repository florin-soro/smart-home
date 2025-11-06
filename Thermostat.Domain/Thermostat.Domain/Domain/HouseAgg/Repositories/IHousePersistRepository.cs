namespace Thermostat.Domain.Domain.HouseAgg.Repositories
{
    public interface IHousePersistRepository
    {   
        Task<HouseRoot?> GetHouseAsync(Guid homeId);
        Task AddHouseAsync(HouseRoot house);
        Task<HouseRoot> UpdateAsync(HouseRoot house);
    }

}