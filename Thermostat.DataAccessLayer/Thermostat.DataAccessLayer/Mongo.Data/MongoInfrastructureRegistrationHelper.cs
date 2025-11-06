using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Thermostat.Application.Common.DbContext;
using Thermostat.Application.EnvironmentalConditions.Rule.Action;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.DbContext;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.HeatingSettings.Repositories;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.House.Repositories;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.Mappings;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories;
using Thermostat.Domain.Domain.HouseAgg.Repositories;
using Thermostat.Domain.Domain.HouseAgg.Rule;

namespace Thermostat.DataAccessLayer.Mongo.Data
{
    public static class MongoInfrastructureRegistrationHelper
    {
        internal static void ConfigureMongo(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(HouseMappingMongoProfile));
            services.AddScoped<IHeatingSettingsRepository, MongoHeatingSettingsRepository>();
            services.AddScoped<IHeatingSettingsReadRepository, MongoHeatingSettingsReadRepository>();
            services.AddScoped<IHousePersistRepository, MongoHouseRepository>();
            services.AddScoped<EnvMeasurementsMongoDbContext>();
            services.AddScoped<IEnvMeasurementsMongoDbContext>(sp =>
                sp.GetRequiredService<EnvMeasurementsMongoDbContext>());
            services.AddScoped<IMeasurementDBUnitOfWork>(sp =>
                sp.GetRequiredService<EnvMeasurementsMongoDbContext>());
            services.AddScoped<MongoDbContextBase>(sp =>
                sp.GetRequiredService<EnvMeasurementsMongoDbContext>());

            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("MongoDB");
                return new MongoClient(connectionString);
            });
            if (!BsonClassMap.IsClassMapRegistered(typeof(ActionDefinitionVO)))
            {
                BsonClassMap.RegisterClassMap<ActionDefinitionVO>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                    cm.SetDiscriminator("action_base");
                });

                BsonClassMap.RegisterClassMap<HeatingControlAction>(cm =>
                {
                    cm.AutoMap();
                    cm.SetDiscriminator("heating_control_action");
                });
            }

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));


        }
    }
}
