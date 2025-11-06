using MongoDB.Driver;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.EntitiesDto;

namespace Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.DbContext
{
    public interface IEnvMeasurementsMongoDbContext
    {
        public IClientSessionHandle? Session { get; }
        public bool InTransaction { get; }
        IMongoCollection<HouseDto> HousesCollection { get; }

        IMongoCollection<HeatingSettingsDto> HeatingSettings { get; }
    }
}