using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext.Configurations
{
    public class RuleParameterConfiguration
        : IEntityTypeConfiguration<RuleParameterEntity>
    {
        public void Configure(EntityTypeBuilder<RuleParameterEntity> builder)
        {
            builder.ToTable("RuleParameters");
            builder.HasKey(rp => rp.Id);

            builder.Property(rp => rp.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
