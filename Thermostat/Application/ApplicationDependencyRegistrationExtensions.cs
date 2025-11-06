using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Thermostat.Application.Common.Handlers;
using Thermostat.Application.Common.Notification;
using Thermostat.Application.EnvironmentalConditions.HeatingSettings.Services;
using Thermostat.Application.EnvironmentalConditions.HeatingSettings.Services.Interfaces;
using Thermostat.Application.EnvironmentalConditions.House.Services;
using Thermostat.Application.EnvironmentalConditions.House.Services.Interfaces;
using Thermostat.Application.EnvironmentalConditions.HouseAgg.Services;
using Thermostat.Application.EnvironmentalConditions.Rule;
using Thermostat.Application.EnvironmentalConditions.Rule.Services;
using Thermostat.Application.EnvironmentalConditions.Rule.Services.Interfaces;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Commands.HeatingControlCommand;
using Thermostat.Domain.Domain.Messages.Events;

namespace Thermostat.Application
{
    public static class ApplicationDependencyRegistrationExtensions
    {
        public static void AddApplicationDependencies(this IServiceCollection services)
        {
            //Commons
            services.AddTransient(typeof(INotificationHandler<DomainEventNotification<AggregateOperationDoneEvent>>), typeof(DomainEventNotificationHandler<AggregateOperationDoneEvent>));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainEventNotificationHandler<>).Assembly));

            //House agg
            services.AddScoped<IPersistHouseService, PersistHouseService>();
            services.AddScoped<IRuleService, RuleService>();
            services.AddScoped<ISensorMeasurementService, SensorMeasurementService>();

            //HeatingSettings agg
            services.AddScoped<IHeatingSettingsWriter, HeatingSettingsWriter>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(HeatingControlCommand).Assembly));

            services.AddScoped<IRuleEngineApplicationService, RuleEngineApplicationService>();
        }
    }
}
