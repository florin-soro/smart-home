using Microsoft.EntityFrameworkCore;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext.Configurations;
namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyAll(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HouseConfiguration());
            modelBuilder.ApplyConfiguration(new HeatingSettingsConfiguration());
            modelBuilder.ApplyConfiguration(new SensorConfiguration());
            modelBuilder.ApplyConfiguration(new SensorMeasurementConfiguration());
            modelBuilder.ApplyConfiguration(new RuleConfiguration());
            modelBuilder.ApplyConfiguration(new RuleDefinitionConfiguration());
            modelBuilder.ApplyConfiguration(new ActionDefinitionConfiguration());
            modelBuilder.ApplyConfiguration(new ActionParameterConfiguration());
            modelBuilder.ApplyConfiguration(new RuleParameterConfiguration());
        }
    }
}

