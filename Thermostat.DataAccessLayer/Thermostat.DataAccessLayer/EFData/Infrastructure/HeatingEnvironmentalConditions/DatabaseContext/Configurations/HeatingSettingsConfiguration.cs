using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext.Configurations
{
    public class HeatingSettingsConfiguration : IEntityTypeConfiguration<HeatingSettingsEntity>
    {
        public void Configure(EntityTypeBuilder<HeatingSettingsEntity> builder)
        {
            builder.ToTable(nameof(EnvMeasurementsWriteSqlDbContext.HeatingSettings));
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Timestamp)
                .IsRequired();
            builder.Property(e => e.TempLow)
                .IsRequired();
            builder.Property(e => e.TempHigh)
                .IsRequired();
            builder.Property(e => e.HumidityAlertThreshold)
                .IsRequired();
        }
    }
}