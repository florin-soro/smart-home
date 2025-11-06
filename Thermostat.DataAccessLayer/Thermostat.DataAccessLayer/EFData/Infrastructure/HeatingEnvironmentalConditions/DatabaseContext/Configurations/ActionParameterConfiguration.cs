using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext.Configurations
{
    public class ActionParameterConfiguration : IEntityTypeConfiguration<ActionParameterEntity>
    {
        public void Configure(EntityTypeBuilder<ActionParameterEntity> builder)
        {
            builder.ToTable("ActionParameters");

            builder.HasKey(ap => ap.Id);

            builder.Property(ap => ap.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("ParameterName");

            builder.Property(ap => ap.Value)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("ParameterValue");

            builder.Property(ap => ap.Type)
                .IsRequired()
                .HasMaxLength(100)
                .HasConversion<string>();

            builder.HasOne(ap => ap.ActionEntity)
                .WithMany(a => a.Parameters)
                .HasForeignKey("ActionDefinitionEntityId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(ap => ap.Name)
                .HasDatabaseName("IX_ActionParameters_Name");
        }
    }
}
