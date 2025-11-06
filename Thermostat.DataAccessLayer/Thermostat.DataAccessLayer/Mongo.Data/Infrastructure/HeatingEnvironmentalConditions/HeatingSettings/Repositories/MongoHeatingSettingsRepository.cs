using AutoMapper;
using MongoDB.Driver;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.DbContext;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.EntitiesDto;
using Thermostat.Domain.Domain.HeatingSettingsAgg;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories;

namespace Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.HeatingSettings.Repositories
{
    public class MongoHeatingSettingsRepository : IHeatingSettingsRepository
    {
        private readonly IMongoCollection<HeatingSettingsDto> _collection;
        private readonly IMapper mapper;

        public MongoHeatingSettingsRepository(IEnvMeasurementsMongoDbContext dbContext, IMapper mapper)
        {
            _collection = dbContext.HeatingSettings;
            this.mapper = mapper;
        }
        //todo 6.	Validate Configuration:
        //Call Mapper.Configuration.AssertConfigurationIsValid() during startup or in tests to catch mapping issues early.
        public async Task AddAsync(HeatingSettingsRoot settings)
        {
            await _collection.InsertOneAsync(mapper.Map<HeatingSettingsDto>(settings));
        }
    }
}
