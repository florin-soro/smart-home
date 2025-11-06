using AutoMapper;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;
using Thermostat.Domain.Domain.HeatingSettingsAgg;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.HeattinhSettings.Repositories
{
    public class SqlHeatingSettingsRepository : IHeatingSettingsRepository
    {
        private EnvMeasurementsWriteSqlDbContext sqlDbContext;
        private IMapper mapper;
        public SqlHeatingSettingsRepository(EnvMeasurementsWriteSqlDbContext measurementsSqlDbContext, IMapper mapper)
        {
            sqlDbContext = measurementsSqlDbContext;
            this.mapper = mapper;
        }

        public async Task AddAsync(HeatingSettingsRoot settings)
        {
            sqlDbContext.HeatingSettings.Add(mapper.Map<HeatingSettingsEntity>(settings));
            await sqlDbContext.SaveChangesAsync();
        }
    }
}
