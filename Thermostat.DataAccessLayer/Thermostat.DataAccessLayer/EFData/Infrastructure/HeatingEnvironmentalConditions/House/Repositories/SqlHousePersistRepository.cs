using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;
using Microsoft.Extensions.Logging;
using Thermostat.Domain.Common;
using Thermostat.DataAccessLayer.Exceptions;
using Thermostat.Domain.Common.Exceptions;
using Thermostat.Domain.Domain.HouseAgg;
using Thermostat.Domain.Domain.HouseAgg.Repositories;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.House.Repositories
{
    public class SqlHousePersistRepository : IHousePersistRepository
    {
        private readonly EnvMeasurementsWriteSqlDbContext _dbContext;
        private readonly IDomainEventDispatcher domainEventDispatcher;
        private readonly IMapper _mapper;
        private readonly ILogger<SqlHousePersistRepository> logger;

        private static readonly Func<EnvMeasurementsWriteSqlDbContext, Guid, Task<HouseEntity?>> _getHouseByIdCompiledQuery =
        EF.CompileAsyncQuery(
            // 2. The lambda parameter must also be the CONCRETE class.
            (EnvMeasurementsWriteSqlDbContext dbContext, Guid houseId) =>
                dbContext.Houses
                    .Include(h => h.Sensors)
                        //.ThenInclude(s => s.Measurements)
                    .Include(h => h.Rules)
                        .ThenInclude(r => r.RuleDefinition)
                            .ThenInclude(a => a.Parameters)
                    .Include(h => h.Rules)
                        .ThenInclude(r => r.ActionDefinition)
                            .ThenInclude(a => a.Parameters)
                    .AsSplitQuery()
                    .FirstOrDefault(h => h.Id == houseId)
        );

        public SqlHousePersistRepository(EnvMeasurementsWriteSqlDbContext dbContext, IDomainEventDispatcher domainEventDispatcher, IMapper mapper, ILogger<SqlHousePersistRepository> logger)
        {
            _dbContext = dbContext;
            this.domainEventDispatcher = domainEventDispatcher;
            _mapper = mapper;
            this.logger = logger;
        }

        public async Task<HouseRoot?> GetHouseAsync(Guid houseId)
        {
            logger.LogInformation($"Looking for house ID-ul {houseId}");
            HouseEntity? result = await _getHouseByIdCompiledQuery(_dbContext, houseId);
            if (result is null)
            {
                logger.LogWarning($"House with ID-ul {houseId} not found in the database.");
                throw new EntityNotFoundException($"House with ID-ul {houseId} not found in the database.");
            }
                logger.LogInformation($"House with ID-ul {houseId} is null :{result is null}");
            return _mapper.Map<HouseRoot>(result);
        }

        public async Task AddHouseAsync(HouseRoot house)
        {
            await _dbContext.Houses.AddAsync(_mapper.Map<HouseEntity>(house, opts => opts.Items["HouseId"] = house.Id));
            await _dbContext.SaveChangesAsync();
            await domainEventDispatcher.DispatchAndClearEventsAsync(new List<AggregateRoot> { house }.AsEnumerable());
        }

        public async Task<HouseRoot> UpdateAsync(HouseRoot house)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(() => RunUpdateTransaction(house));
        }

        private async Task<HouseRoot> RunUpdateTransaction(HouseRoot house)
        {
            if (!_dbContext.ChangeTracker.Entries<HouseEntity>().Any(h=>h.Entity.Id == house.Id))
            {
                throw new InfrastructureException($"House with the specified ID is not being tracked by the context. Please call {nameof(GetHouseAsync)} first, then update the House");
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Load existing tracked house entity
                var existingHouse = await _dbContext.Houses.FindAsync(house.Id);

                _mapper.Map(house, existingHouse);

                #region Debug logging to Console
                foreach (var entry in _dbContext.ChangeTracker.Entries())
                {
                    Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");

                    if (entry.State == EntityState.Modified)
                    {
                        foreach (var prop in entry.OriginalValues.Properties)
                        {
                            var original = entry.OriginalValues[prop]?.ToString();
                            var current = entry.CurrentValues[prop]?.ToString();

                            if (original != current)
                                Console.WriteLine($"  {prop.Name}: {original} => {current}");
                        }
                    }
                }
                #endregion

                var newMeasurements = house.Sensors
                        .Where(s => s.NewMeasurements.Any())
                        .SelectMany(sensor => sensor.Measurements.Select(m => new SensorMeasurementEntity
                        {
                            HouseRootId = house.Id,
                            SensorEntityId = sensor.Id,
                            Value = m.Value,
                            Timestamp = m.Timestamp
                        }));

                if (newMeasurements.Any())
                {
                    // Add new measurements
                    await _dbContext.SensorMeasurements.AddRangeAsync(newMeasurements);
                }

                await _dbContext.SaveChangesAsync();

                house = _mapper.Map<HouseRoot>(existingHouse);

                await domainEventDispatcher.DispatchAndClearEventsAsync(new List<AggregateRoot> { house }.AsEnumerable());

                transaction.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error during update: {ex.Message}");
                transaction.Rollback();
                throw new InfrastructureException($"Error during update: {ex.Message}",ex);
            }
            return house;
        }

        //for debug purposes
        //var json0 = JsonSerializer.Serialize(house, new JsonSerializerOptions
        //{
        //    WriteIndented = true, // optional: makes output pretty
        //    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles // avoid circular refs
        //});
        //Console.WriteLine("house domain entity before mapping \n" + json0);
        //var s = existingHouse.Sensors.FirstOrDefault(x => x.Id == house.NewSensors.First().Id);
        //var json2 = JsonSerializer.Serialize(existingHouse, new JsonSerializerOptions
        //{
        //    WriteIndented = true, // optional: makes output pretty
        //    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles // avoid circular refs
        //});
        //Console.WriteLine("existingHouse before mapping \n" + json2);


        //var json1 = JsonSerializer.Serialize(existingHouse, new JsonSerializerOptions
        //{
        //    WriteIndented = true, // optional: makes output pretty
        //    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles // avoid circular refs
        //});
        //Console.WriteLine("existingHouse after mapping \n" + json1);

        //foreach (var entry in _dbContext.ChangeTracker.Entries<Entities.SensorEntity>())
        //{
        //    if (entry.State == EntityState.Added && entry.Entity.Id == Guid.Empty)
        //    {
        //        entry.Entity.Id = Guid.NewGuid();
        //    }
        //}

        //foreach (var entry in _dbContext.ChangeTracker.Entries<Entities.RuleEntity>())
        //{
        //    if (entry.State == EntityState.Added && entry.Entity.Id == Guid.Empty)
        //    {
        //        entry.Entity.Id = Guid.NewGuid();
        //    }
        //}
        //var houseEntity = _mapper.Map<HouseEntity>(house);

        //_dbContext.Houses.Attach(houseEntity);
        //_dbContext.Entry(houseEntity).State = EntityState.Modified;

        //foreach (var entry in _dbContext.ChangeTracker.Entries())
        //{
        //    var json = JsonSerializer.Serialize(entry.Entity, new JsonSerializerOptions
        //    {
        //        WriteIndented = true, // optional: makes output pretty
        //        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles // avoid circular refs
        //    });

        //    Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}\n{json}\n");
        //    Console.WriteLine("===============================================================");
        //    Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");

        //    if (entry.State == EntityState.Modified)
        //    {
        //        foreach (var prop in entry.OriginalValues.Properties)
        //        {
        //            var original = entry.OriginalValues[prop]?.ToString();
        //            var current = entry.CurrentValues[prop]?.ToString();

        //            if (original != current)
        //                Console.WriteLine($"  {prop.Name}: {original} => {current}");
        //        }
        //    }
        //}

    }
}
