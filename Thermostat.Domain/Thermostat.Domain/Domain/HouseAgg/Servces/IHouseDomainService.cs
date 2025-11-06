namespace Thermostat.Domain.Domain.HouseAgg.Servces
{
    public interface IHouseDomainService
    {
        Task<bool> ExistsAsync(Guid houseId, Guid sensorId);
    }
}
