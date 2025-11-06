using AutoMapper.EquivalencyExpression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.HeattinhSettings.Repositories;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.House.Repositories;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Mappings;
using Thermostat.DataAccessLayer.EFData.Infrastructure.Services;
using Thermostat.Domain.Common;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories;
using Thermostat.Domain.Domain.House.Repositories;
using Thermostat.Domain.Domain.HouseAgg.Repositories;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure
{
    public static class SqlInfrastructureRegistrationHelper
    {
        internal static void ConfigureSql(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IHeatingSettingsRepository, SqlHeatingSettingsRepository>();
            services.AddScoped<IHeatingSettingsReadRepository, SqlHeatingSettingsReadRepository>();
            services.AddScoped<IHousePersistRepository, SqlHousePersistRepository>();
            services.AddScoped<IHouseQueryRepository, SqlHouseQueryRepository>();
            services.AddDbContextPool<EnvMeasurementsWriteSqlDbContext>((serviceProvideroptions, options) =>
            {
                var configuration = serviceProvideroptions.GetRequiredService<IConfiguration>();
                options.UseSqlServer(
                    configuration.GetConnectionString("SqlDB"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(15),
                errorNumbersToAdd: null)
                        );
            });

            services.AddDbContextPool<EnvMeasurementsReadSqlDbContext>((serviceProvideroptions, options) =>
            {
                var configuration = serviceProvideroptions.GetRequiredService<IConfiguration>();
                options
                .UseSqlServer(
                    configuration.GetConnectionString("SqlDB"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(15),
                errorNumbersToAdd: null))
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddAutoMapper(cfg =>
            {
                cfg.AddCollectionMappers();
            }, typeof(HouseMappingSqlProfile));
        }
    }
}
