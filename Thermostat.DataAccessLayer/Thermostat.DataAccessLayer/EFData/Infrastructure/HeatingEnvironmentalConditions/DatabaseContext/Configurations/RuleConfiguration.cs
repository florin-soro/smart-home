using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;
namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext.Configurations
{
    public class RuleConfiguration : IEntityTypeConfiguration<RuleEntity>
    {
        public void Configure(EntityTypeBuilder<RuleEntity> builder)
        {
            builder.ToTable("Rules");

            builder.HasKey(r => r.Id);

            builder.Property(p => p.Name).HasColumnName("Name").IsRequired();

            builder.HasOne<SensorEntity>()
                           .WithMany() 
                           .HasForeignKey(r=>r.SensorId)
                           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r=>r.RuleDefinition)
                   .WithOne()
                   .HasForeignKey<RuleDefinitionEntity>(r=>r.RuleEntityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.ActionDefinition)
                   .WithOne()
                   .HasForeignKey<ActionDefinitionEntity>(r=>r.RuleEntityId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
