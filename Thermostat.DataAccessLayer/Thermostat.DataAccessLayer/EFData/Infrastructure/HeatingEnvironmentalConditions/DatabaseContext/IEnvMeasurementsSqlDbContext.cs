using Microsoft.EntityFrameworkCore;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;
namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext
{
    public class EnvMeasurementsWriteSqlDbContext : DbContext
    {
        public EnvMeasurementsWriteSqlDbContext() : base() { }
        public EnvMeasurementsWriteSqlDbContext(DbContextOptions<EnvMeasurementsWriteSqlDbContext> options) : base(options) { }
        public DbSet<HouseEntity> Houses { get; protected set; }
        public DbSet<HeatingSettingsEntity> HeatingSettings { get; protected set; }
        public DbSet<RuleEntity> Rules { get; protected set; }
        public DbSet<RuleDefinitionEntity> RuleDefinitionEntities { get; protected set; }
        public DbSet<ActionDefinitionEntity> ActionDefinitionEntities { get; protected set; }
        public DbSet<RuleParameterEntity> RuleParameters { get; protected set; }
        public DbSet<ActionParameterEntity> ActionParameters { get; protected set; }
        public DbSet<SensorEntity> Sensors { get; protected set; }
        public DbSet<SensorMeasurementEntity> SensorMeasurements { get; protected set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyAll();

            SeedData(modelBuilder);
        }
        private static void SeedData(ModelBuilder modelBuilder)
        {
            var houseId = Guid.Parse("90A6909D-5D14-4220-B035-B60926EDD2D4");
            var sensorTempId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");
            var sensorHumId = Guid.Parse("B1C2D3E4-F5A6-7890-ABCD-EF1234567890");
            var actionDefId = Guid.Parse("90A6909D-5D14-4220-B035-B60926EDD2D5");
            var ruleDefId = Guid.Parse("90A6909D-5D14-4220-B035-B60926EDD2D7");
            var ruleId = Guid.Parse("90A6909D-5D14-4220-B035-B60926EDD2D6");
            var actionParamId = Guid.Parse("90A6909D-5D14-4220-B035-B60926EDD2D7");
            var ruleParamId = Guid.Parse("90A6909D-5D14-4220-B035-B60926EDD2D9");

            modelBuilder.Entity<HouseEntity>().HasData(new HouseEntity
            {
                Id = houseId,
                Name = "Default House"
            });

            modelBuilder.Entity<SensorEntity>().HasData(
                new SensorEntity
                {
                    Id = sensorTempId,
                    Type = "Temperature",
                    Unit = "Celsius",
                    RoomName = "Living Room",
                    RoomArea = 20.0,
                    RoomType = "LivingRoom",
                    HouseRootId = houseId
                },
                new SensorEntity
                {
                    Id = sensorHumId,
                    Type = "Humidity",
                    Unit = "Percent",
                    RoomName = "Bedroom",
                    RoomArea = 15.0,
                    RoomType = "Bedroom",
                    HouseRootId = houseId
                }
            );

            modelBuilder.Entity<RuleEntity>().HasData(new RuleEntity
            {
                Id = ruleId,
                Name = "Stop Heating Rule if temp >= 23°C",
                SensorId = sensorTempId,
                HouseRootId = houseId,
                Enabled = true
            });

            modelBuilder.Entity<RuleDefinitionEntity>().HasData(new RuleDefinitionEntity
            {
                Id = ruleDefId,
                Type = RuleDefinitionType.GreaterThan,
                RuleEntityId = ruleId,
            });

            modelBuilder.Entity<RuleParameterEntity>().HasData(new RuleParameterEntity
            {
                Id = ruleParamId,
                Name = "Threshold",
                Value = "23.0",
                Type = "System.Double",
                RuleDefinitionEntityId = ruleDefId
            });

            modelBuilder.Entity<ActionDefinitionEntity>().HasData(new ActionDefinitionEntity
            {
                Id = actionDefId,
                Type = "HeatingControl",
                RuleEntityId = ruleId,
                CreatedAt = new DateTime(2015, 07, 03)
            });

            modelBuilder.Entity<ActionParameterEntity>().HasData(new ActionParameterEntity
            {
                Id = actionParamId,
                Name = "Command",
                Value = "Stop",
                Type = "System.String",
                ActionDefinitionEntityId = actionDefId
            });
        }
    }

    public class EnvMeasurementsReadSqlDbContext : DbContext
    {
        public DbSet<HouseEntity> Houses { get; protected set; }
        public DbSet<HeatingSettingsEntity> HeatingSettings { get; protected set; }
        public DbSet<RuleEntity> Rules { get; protected set; }
        public DbSet<RuleDefinitionEntity> RuleDefinitionEntities { get; protected set; }
        public DbSet<ActionDefinitionEntity> ActionDefinitionEntities { get; protected set; }
        public DbSet<RuleParameterEntity> RuleParameters { get; protected set; }
        public DbSet<ActionParameterEntity> ActionParameters { get; protected set; }
        public DbSet<SensorEntity> Sensors { get; protected set; }
        public DbSet<SensorMeasurementEntity> SensorMeasurements { get; protected set; }
        public EnvMeasurementsReadSqlDbContext(DbContextOptions<EnvMeasurementsReadSqlDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyAll();
        }
    }
}

