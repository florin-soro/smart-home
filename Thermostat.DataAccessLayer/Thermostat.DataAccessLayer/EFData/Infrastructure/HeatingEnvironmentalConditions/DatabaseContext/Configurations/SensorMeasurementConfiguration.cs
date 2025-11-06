using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;
namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext.Configurations
{
    public class SensorMeasurementConfiguration : IEntityTypeConfiguration<SensorMeasurementEntity>
    {
        public void Configure(EntityTypeBuilder<SensorMeasurementEntity> builder)
        {
            builder.ToTable("SensorMeasurements");

            builder.HasKey(sm => new { sm.HouseRootId, sm.SensorEntityId, sm.Timestamp });

            builder.Property(sm => sm.Value)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(sm => sm.Timestamp)
                .IsRequired();

            builder.HasOne(m=>m.Sensor)
                .WithMany(s=>s.Measurements)
                .HasForeignKey(m => m.SensorEntityId)
                .HasPrincipalKey(s => s.Id)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasOne<HouseEntity>()
                .WithMany()
                .HasForeignKey(m => m.HouseRootId)
                .HasPrincipalKey(s => s.Id) 
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(sm => new { sm.SensorEntityId, sm.Timestamp })
                .IsDescending(false, true) 
                .HasDatabaseName("IX_SensorMeasurements_SensorId_Timestamp");

            builder.HasIndex(sm => sm.Timestamp)
                .HasDatabaseName("IX_SensorMeasurements_Timestamp");
        }
    }
}
