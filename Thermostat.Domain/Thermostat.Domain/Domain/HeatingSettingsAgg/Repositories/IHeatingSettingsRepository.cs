namespace Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories
{
    public interface IHeatingSettingsRepository
    {
        Task AddAsync(HeatingSettingsRoot settings);
    }
}
