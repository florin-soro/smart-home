using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;
namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext.Configurations
{
    public class SensorConfiguration : IEntityTypeConfiguration<SensorEntity>
    {
        public void Configure(EntityTypeBuilder<SensorEntity> builder)
        {
            builder.ToTable("Sensors");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("SensorType");

            builder.Property(s => s.Unit)
                .HasMaxLength(20)
                .IsRequired(true) 
                .HasColumnName("MeasurementUnit");

            builder.Property(p => p.RoomName).HasColumnName("RoomName").IsRequired();
            builder.Property(p => p.RoomArea).HasColumnName("RoomArea").IsRequired();
            builder.Property(p => p.RoomType).HasColumnName("RoomType").IsRequired();
        }
    }
}
