using Microsoft.Extensions.DependencyInjection;
using Thermostat.Domain.Common;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext;
using Thermostat.Domain.Domain.HouseAgg.Sensor.Services;

namespace Thermostat.Domain.Extensions
{
    public static class DomainDependencyRegistrationExtensions
    {
        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ISensorMeasurementsDomainService, SensorMeasurementsDomainService>();
            services.AddScoped<IRuleContextFactory, HouseRuleContextFactory>();
        }
    }
}
