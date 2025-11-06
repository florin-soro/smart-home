using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext.Configurations
{
    public class RuleDefinitionConfiguration : IEntityTypeConfiguration<RuleDefinitionEntity>
    {
        public void Configure(EntityTypeBuilder<RuleDefinitionEntity> builder)
        {
            builder.ToTable("RuleDefinitions");
            builder.HasKey(rd => rd.Id);

            // Configure parameters
            builder.HasMany(rd => rd.Parameters)
                   .WithOne()
                   .HasForeignKey("RuleDefinitionEntityId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(rd => rd.Type)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasConversion<string>();
        }
    }
}
