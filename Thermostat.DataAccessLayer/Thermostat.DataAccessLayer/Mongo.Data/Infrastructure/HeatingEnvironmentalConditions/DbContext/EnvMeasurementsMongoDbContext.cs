using MongoDB.Driver;
using Thermostat.Application;
using Thermostat.Application.Common.DbContext;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.EntitiesDto;

namespace Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.DbContext
{
    public class EnvMeasurementsMongoDbContext : MongoDbContextBase, IEnvMeasurementsMongoDbContext, IMeasurementDBUnitOfWork
    {//todo validate settings at startup
        public EnvMeasurementsMongoDbContext(IMongoClient mongoClient, IGenericSettings genneralSettings) : base(mongoClient, genneralSettings.MeasurementDbName)
        {
        }


        public IMongoCollection<HouseDto> HousesCollection=> _database.GetCollection<HouseDto>(nameof(HouseDto));

        public IMongoCollection<HeatingSettingsDto> HeatingSettings => _database.GetCollection<HeatingSettingsDto>(nameof(HeatingSettingsDto));

        public IClientSessionHandle? Session => _session;

        public bool InTransaction => _session is not null;
    }
}
