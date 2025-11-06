using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;
namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext.Configurations
{
    public class HouseConfiguration : IEntityTypeConfiguration<HouseEntity>
    {
        public void Configure(EntityTypeBuilder<HouseEntity> builder)
        {
            builder.ToTable(nameof(EnvMeasurementsWriteSqlDbContext.Houses));

            builder.HasKey(h => h.Id);

            builder.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
            .HasMany(h => h.Sensors)
            .WithOne() 
            .HasForeignKey(s => s.HouseRootId) 
            .OnDelete(DeleteBehavior.Cascade);

            builder
            .HasMany(h => h.Rules)
            .WithOne() 
            .HasForeignKey(r => r.HouseRootId) 
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
