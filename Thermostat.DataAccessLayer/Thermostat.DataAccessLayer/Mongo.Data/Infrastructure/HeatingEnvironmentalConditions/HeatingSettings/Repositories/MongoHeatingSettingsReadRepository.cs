using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Thermostat.Api.Config;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.DbContext;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.EntitiesDto;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories;

namespace Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.HeatingSettings.Repositories
{
    public class MongoHeatingSettingsReadRepository : IHeatingSettingsReadRepository
    {
        private readonly IEnvMeasurementsMongoDbContext dbContext;
        private readonly IThermostatSettings thermostatSettings;

        public MongoHeatingSettingsReadRepository(IEnvMeasurementsMongoDbContext dbContext, IThermostatSettings thermostatSettings)
        {
            this.dbContext = dbContext;
            this.thermostatSettings = thermostatSettings;
        }
        public async Task<double> GetTemperatureHighAsync(DateTime at)
        {
            var filter = Builders<HeatingSettingsDto>.Filter.Lt(x => x.Timestamp, at);
            var sort = Builders<HeatingSettingsDto>.Sort.Descending(x => x.Timestamp);

            var heatingSetting = await dbContext.HeatingSettings
               .Find(filter)
               .Sort(sort)
               .Project(x => new { x.TempHigh })
               .FirstOrDefaultAsync();

            if (heatingSetting == null)
            {
                return thermostatSettings.TempHigh;
            }

            return heatingSetting.TempHigh;
        }


        public async Task<double> GetTemperatureLowAsync(DateTime at)
        {
            var filter = Builders<HeatingSettingsDto>.Filter.Lt(x => x.Timestamp, at);
            var sort = Builders<HeatingSettingsDto>.Sort.Descending(x => x.Timestamp);

            var heatingSetting = await dbContext.HeatingSettings
               .Find(filter)
               .Sort(sort)
               .Project(x => new { x.TempLow })
               .FirstOrDefaultAsync();

            if (heatingSetting == null)
            {
                return thermostatSettings.TempLow;
            }

            return heatingSetting.TempLow;
        }

        public async Task<double> GetOutlierTemperatureThresholdValueAsync(DateTime at)
        {
            var filter = Builders<HeatingSettingsDto>.Filter.Lt(x => x.Timestamp, at);
            var sort = Builders<HeatingSettingsDto>.Sort.Descending(x => x.Timestamp);

            var heatingSetting = await dbContext.HeatingSettings
               .Find(filter)
            .Sort(sort)
               .Project(x => new { x.HumidityAlertThreshold })
            .FirstOrDefaultAsync();

            if (heatingSetting == null)
            {
                return thermostatSettings.HumidityThreshold;
            }

            return heatingSetting.HumidityAlertThreshold;
        }
    }
}
