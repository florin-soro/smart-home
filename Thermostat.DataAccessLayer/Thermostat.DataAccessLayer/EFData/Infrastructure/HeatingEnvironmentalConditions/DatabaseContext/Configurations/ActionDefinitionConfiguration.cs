using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext.Configurations
{
    public class ActionDefinitionConfiguration : IEntityTypeConfiguration<ActionDefinitionEntity>
    {
        public void Configure(EntityTypeBuilder<ActionDefinitionEntity> builder)
        {
            builder.ToTable("ActionDefinitions");
            builder.HasKey(ad => ad.Id);

            builder.Property(rd => rd.Type)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasConversion<string>();

            builder.HasMany(ad => ad.Parameters)
                   .WithOne()
                   .HasForeignKey("ActionDefinitionEntityId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
