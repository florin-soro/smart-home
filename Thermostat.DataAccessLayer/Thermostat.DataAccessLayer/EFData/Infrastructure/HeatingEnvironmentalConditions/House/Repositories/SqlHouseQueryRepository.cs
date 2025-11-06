using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;
using Thermostat.Domain.Common;
using Thermostat.Domain.Domain.House.Repositories;
using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg;
using Thermostat.Domain.Domain.HouseAgg.Sensor;
namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.House.Repositories
{
    public class SqlHouseQueryRepository : IHouseQueryRepository
    {
        private readonly EnvMeasurementsReadSqlDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SqlHouseQueryRepository> logger;

        public SqlHouseQueryRepository(EnvMeasurementsReadSqlDbContext dbContext, IDomainEventDispatcher domainEventDispatcher, IMapper mapper, ILogger<SqlHouseQueryRepository> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            this.logger = logger;
        }

        private static readonly Func<EnvMeasurementsReadSqlDbContext, Guid,DateTime,DateTime, Task<HouseEntity?>> _getHouseByIdWithMeasurementsCompiledQuery =
EF.CompileAsyncQuery(
    // 2. The lambda parameter must also be the CONCRETE class.
    (EnvMeasurementsReadSqlDbContext dbContext, Guid houseId, DateTime start, DateTime end) =>
        dbContext.Houses
            .Include(h => h.Sensors)
                .ThenInclude(s => s.Measurements.Where(m => m.Timestamp >= start && m.Timestamp <= end))
            .Include(h => h.Rules)
                .ThenInclude(r => r.RuleDefinition)
                    .ThenInclude(a => a.Parameters)
            .Include(h => h.Rules)
                .ThenInclude(r => r.ActionDefinition)
                    .ThenInclude(a => a.Parameters)
            .AsSplitQuery()
            .FirstOrDefault(h => h.Id == houseId));

        public async Task<SensorMeasurementVO?> GetLastMeasurementAsync(Guid houseId, Guid sensorId, DateTime timestamp)
        {
            var measurement = await _dbContext.SensorMeasurements
                .Where(m => m.HouseRootId == houseId &&
                           m.SensorEntityId == sensorId &&
                           m.Timestamp < timestamp)
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefaultAsync();

            return measurement != null
                ? new SensorMeasurementVO(measurement.Timestamp, measurement.Value)
                : null;
        }

        public async Task<HouseRoot> GetHouseWithSensorMeasurements(Guid houseId, DateTime start, DateTime end)
        {
            HouseEntity? result = await _getHouseByIdWithMeasurementsCompiledQuery(_dbContext, houseId,start,end);

            if (result is null)
            {
                logger.LogWarning($"House with ID-ul {houseId} not found in the database.");
                return null;
            }
            logger.LogInformation($"House with ID-ul {houseId} is null :{result is null}");

            return _mapper.Map<HouseRoot>(result);  
        }

        public async Task<List<SensorMeasurementVO>> GetMeasurementsAsync(Guid houseId, Guid sensorId, DateTime start, DateTime end)
        {
            var measurements = await _dbContext.SensorMeasurements
                .Where(m => m.HouseRootId == houseId &&
                           m.SensorEntityId == sensorId &&
                           m.Timestamp >= start &&
                           m.Timestamp <= end)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return measurements.Select(m =>
                new SensorMeasurementVO(m.Timestamp, m.Value))
                .ToList();
        }

        public async Task<SensorMeasurementVO> GetPreviousMeasurementStartingFromAsync(Guid houseId, Guid sensorId, DateTime timestamp)
        {
            var measurement = await _dbContext.SensorMeasurements
                .Where(m => m.HouseRootId == houseId &&
                           m.SensorEntityId == sensorId &&
                           m.Timestamp <= timestamp)
                .OrderByDescending(m => m.Timestamp)
                .FirstOrDefaultAsync();

            if (measurement == null)
                return null;

            return new SensorMeasurementVO(measurement.Timestamp, measurement.Value);
        }
        
        public async Task<List<Domain.Domain.HouseAgg.Rule.RuleEntity>> GetRulesAsync(Guid houseId, Guid sensorId, bool includeDisabled)
        {
            var query = _dbContext.Rules
                .Include(r=>r.RuleDefinition)
                    .ThenInclude(rd=>rd.Parameters)
                .Include(r=>r.ActionDefinition)
                    .ThenInclude(ad=>ad.Parameters)
                .Where(r => r.HouseRootId == houseId && r.SensorId == sensorId);

            if (!includeDisabled)
            {
                query = query.Where(r => r.Enabled);
            }
            var sensor = await _dbContext.Sensors.FirstOrDefaultAsync(s => s.Id == sensorId && s.HouseRootId == houseId);
            return _mapper.Map<List<Domain.Domain.HouseAgg.Rule.RuleEntity>>(await query.ToListAsync(), opt => opt.Items["Sensor"] = sensor);
        }
    }
}
