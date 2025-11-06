using Microsoft.EntityFrameworkCore;
using Thermostat.Api.Config;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.HeattinhSettings.Repositories
{
    public class SqlHeatingSettingsReadRepository : IHeatingSettingsReadRepository
    {
        private readonly EnvMeasurementsReadSqlDbContext dbContext;
        private readonly IThermostatSettings thermostatSettings;

        public SqlHeatingSettingsReadRepository(EnvMeasurementsReadSqlDbContext dbContext, IThermostatSettings thermostatSettings)
        {
            this.dbContext = dbContext;
            this.thermostatSettings = thermostatSettings;
        }
        public async Task<double> GetOutlierTemperatureThresholdValueAsync(DateTime at)
        {
            var heatingSetting = await dbContext.HeatingSettings
                .Where(x => x.Timestamp < at)
                .OrderByDescending(x => x.Timestamp)
                .Select(x => x.HumidityAlertThreshold)
                .FirstOrDefaultAsync();

            return heatingSetting == default
                ? thermostatSettings.HumidityThreshold
                : heatingSetting;
        }

        public async Task<double> GetTemperatureHighAsync(DateTime at)
        {
            var tempHigh = await dbContext.HeatingSettings
                .Where(x => x.Timestamp < at)
                .OrderByDescending(x => x.Timestamp)
                .Select(x => x.TempHigh)
                .FirstOrDefaultAsync();
            return tempHigh == default
                ? thermostatSettings.TempHigh
                : tempHigh;
        }
        public async Task<double> GetTemperatureLowAsync(DateTime at)
        {
            var tempLow = await dbContext.HeatingSettings
                .Where(x => x.Timestamp < at)
                .OrderByDescending(x => x.Timestamp)
                .Select(x => x.TempLow)
                .FirstOrDefaultAsync();

            return tempLow == default
                ? thermostatSettings.TempLow
                : tempLow;
        }
    }
}
