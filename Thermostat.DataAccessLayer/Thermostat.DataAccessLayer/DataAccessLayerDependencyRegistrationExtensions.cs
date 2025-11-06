using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Thermostat.Application.EnvironmentalConditions.HeatingSettings.Services.Interfaces;
using Thermostat.Application.EnvironmentalConditions.House.Actions;
using Thermostat.Application.EnvironmentalConditions.Rule.Action;
using Thermostat.DataAccessLayer.EFData.Infrastructure;
using Thermostat.DataAccessLayer.Mongo.Data;
using Thermostat.DataAccessLayer.Shared.Actions.HeatingControl;
using Thermostat.DataAccessLayer.Shared.ExternalCommands;

namespace Thermostat.DataAccessLayer
{
    public static class DataAccessLayerDependencyRegistrationExtensions
    {
        public static void AddSqlDataAccessLayerDependencies(this IServiceCollection services)
        {
            services.AddCoreDataAccessLayerDependencies();
            services.ConfigureSql();
        }

        public static void AddMongoDataAccessLayerDependencies(this IServiceCollection services)
        {
            services.AddCoreDataAccessLayerDependencies();
            services.ConfigureMongo();
        }

        internal static void AddCoreDataAccessLayerDependencies(this IServiceCollection services)
        {
            services.AddScoped<IHeatingDeviceControllerService, HttpHeatingDeviceController>();

            services.AddScoped(sp =>
            {
                var builder = new ActionExecutorRegistryBuilder()
                    .Register<HeatingControlAction>(new HeatingControlActionExecutor(sp.GetRequiredService<IMediator>()));//
                return builder.Build();
            });
        }
    }
}
