using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Thermostat.Application.Common.Exceptions;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.DbContext;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.EntitiesDto;
using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg;
using Thermostat.Domain.Domain.HouseAgg.Exceptions;
using Thermostat.Domain.Domain.HouseAgg.Repositories;
using Thermostat.Domain.Domain.HouseAgg.Rule;
using Thermostat.Domain.Domain.HouseAgg.Sensor;
using EntityNotFoundException = Thermostat.Domain.Common.Exceptions.EntityNotFoundException;

namespace Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.House.Repositories
{
    public class MongoHouseRepository : IHousePersistRepository
    {
        private readonly IMongoCollection<HouseDto> _houses;
        private readonly IEnvMeasurementsMongoDbContext dbContext;
        private readonly IMapper mapper;//todo setup mapper

        public MongoHouseRepository(IEnvMeasurementsMongoDbContext dbContext, IMapper mapper)
        {
            _houses = dbContext.HousesCollection;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        // Domain interface returns List<SensorMeasurement> (domain VO),
        // internally we load SensorMeasurementDto and map back
        public async Task<List<SensorMeasurementVO>> GetMeasurementsAsync(Guid houseId, Guid sensorId, DateTime start, DateTime end)
        {
            var filter = Builders<HouseDto>.Filter.Eq(h => h.Id, houseId);

            var projection = Builders<HouseDto>.Projection.Expression(h =>
                           h.Sensors
                               .Where(s => s.Id == sensorId)
                               .SelectMany(s => s.Measurements)
                               .Where(m => m.Timestamp >= start && m.Timestamp <= end)
                               .ToList());

            var measurementsDto = await _houses.Find(filter).Project(projection).FirstOrDefaultAsync();

            var measurementsDomain = mapper.Map<List<SensorMeasurementVO>>(measurementsDto);
            return measurementsDomain;
        }

        // AddMeasurementAsync accepts a domain VO, map it to DTO and push
        private async Task AddMeasurementsAsync(Guid houseId, Guid sensorId, IReadOnlyCollection<SensorMeasurementVO> sensorMeasurement)
        {
            var filter = Builders<HouseDto>.Filter.And(
                Builders<HouseDto>.Filter.Eq(h => h.Id, houseId),
                Builders<HouseDto>.Filter.ElemMatch(h => h.Sensors, s => s.Id == sensorId)
            );

            var measurementDtos = mapper.Map<List<SensorMeasurementDto>>(sensorMeasurement);

            var update = Builders<HouseDto>.Update.PushEach("Sensors.$.Measurements", measurementDtos);

            if (dbContext.InTransaction)
            {
                await _houses.UpdateOneAsync(dbContext.Session, filter, update);
            }
            else
            {
                await _houses.UpdateOneAsync(filter, update);
            }
        }

        //to GetHouse returns domain aggregate, so load DTO and map to domain
        public async Task<HouseRoot?> GetHouseAsync(Guid homeId)
        {
            var dto = await _houses.Find(h => h.Id == homeId).SingleOrDefaultAsync();
            if (dto == null) throw new EntityNotFoundException($"House with ID {homeId} not found.");

            return mapper.Map<HouseRoot>(dto);
        }

        public async Task<SensorMeasurementVO?> GetLastMeasurementAsync(Guid houseId, Guid sensorId, DateTime timestamp)
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument("_id",  new BsonBinaryData(houseId, GuidRepresentation.Standard))),
                new BsonDocument("$unwind", "$Sensors"),
                new BsonDocument("$match", new BsonDocument("Sensors._id", new BsonBinaryData(sensorId, GuidRepresentation.Standard))),
                new BsonDocument("$unwind", "$Sensors.Measurements"),
                new BsonDocument("$match", new BsonDocument("Sensors.Measurements.Timestamp", new BsonDocument("$lt", timestamp))),
                new BsonDocument("$sort", new BsonDocument("Sensors.Measurements.Timestamp", -1)),
                new BsonDocument("$limit", 1),
                new BsonDocument("$replaceWith", "$Sensors.Measurements")
            };

            var result = await _houses.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();

            var measurementsDto = result == null
                ? null
                : BsonSerializer.Deserialize<SensorMeasurementDto>(result);

            if (measurementsDto == null)
                return null;

            var measurementDomain = mapper.Map<SensorMeasurementVO>(measurementsDto);
            return measurementDomain;
        }

        public async Task AddHouseAsync(HouseRoot house)
        {

            if (dbContext.InTransaction)
            {
                await _houses.InsertOneAsync(dbContext.Session, mapper.Map<HouseDto>(house));
            }
            else
            {
                await _houses.InsertOneAsync(mapper.Map<HouseDto>(house));
            }

        }

        private async Task AddSensorsToHouseAsync(Guid houseId, IReadOnlyCollection<SensorEntity> sensors)
        {
            var filterHouseId = Builders<HouseDto>.Filter.Eq(h => h.Id, houseId);
            var sensorDtos = sensors.Select(mapper.Map<SensorDto>).ToList();
            var update = Builders<HouseDto>.Update.PushEach("Sensors", sensorDtos);
            UpdateResult result = null;
            if (dbContext.InTransaction)
            {
                result = await _houses.UpdateOneAsync(dbContext.Session, filterHouseId, update);
            }
            else
            {
                result = await _houses.UpdateOneAsync(filterHouseId, update);
            }

            if (!result.IsAcknowledged || !(result.ModifiedCount > 0))
            {
                throw new SensorPersistenceException("Failed to add sensors to house.");
            }
        }

        public async Task<SensorMeasurementVO> GetPreviousMeasurementStartingFromAsync(Guid houseId, Guid sensorId, DateTime timestamp)
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$match", new BsonDocument("_id",  new BsonBinaryData(houseId, GuidRepresentation.Standard))),
                new BsonDocument("$unwind", "$Sensors"),
                new BsonDocument("$match", new BsonDocument("Sensors._id", new BsonBinaryData(sensorId, GuidRepresentation.Standard))),
                new BsonDocument("$unwind", "$Sensors.Measurements"),
                new BsonDocument("$match", new BsonDocument("Sensors.Measurements.Timestamp", new BsonDocument("$lte", timestamp))),
                new BsonDocument("$sort", new BsonDocument("Sensors.Measurements.Timestamp", -1)),
                new BsonDocument("$limit", 1),
                new BsonDocument("$replaceWith", "$Sensors.Measurements")
            };

            var result = await _houses.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();

            var measurementsDto = result == null
                ? null
                : BsonSerializer.Deserialize<SensorMeasurementDto>(result);

            if (measurementsDto == null)
                return null;

            // Map the first (latest) DTO to domain VO
            var measurementDomain = mapper.Map<SensorMeasurementVO>(measurementsDto);
            return measurementDomain;
        }

        private async Task AddRulesAsync(Guid houseId, IReadOnlyCollection<RuleEntity> rules)
        {
            if (rules.Count == 0)
            {
                throw new InvalidlParameterException("Rules collection cannot be empty.");
            }
            var ruleDtos = rules.Select(mapper.Map<RuleDto>).ToList();

            var filter = Builders<HouseDto>.Filter.Eq(h => h.Id, houseId);

            var update = Builders<HouseDto>.Update.PushEach("Rules", ruleDtos);
            UpdateResult result = null;

            if (dbContext.InTransaction)
                result = await _houses.UpdateOneAsync(dbContext.Session, filter, update);
            else
                result = await _houses.UpdateOneAsync(filter, update);

            if (!result.IsAcknowledged || result.ModifiedCount == 0)
            {
                throw new EntityNotFoundException($"Failed to add rules to house {houseId} because house not found");
            }
        }

        public async Task<HouseRoot> UpdateAsync(HouseRoot house)
        {
            if (house.HasNewSensorsAdded)
            {
                await AddSensorsToHouseAsync(house.Id, house.NewSensors);
            }

            if (house.HasNewMeasurementsAdded)
            {
                foreach (var sensorWithNewData in house.Sensors.Where(s => s.NewMeasurements.Any()))
                {
                    await AddMeasurementsAsync(house.Id, sensorWithNewData.Id, sensorWithNewData.NewMeasurements);
                }
            }

            if (house.HasNewRulesAdded)
            {
                await AddRulesAsync(house.Id, house.NewRules);
            }

            house.ClearNewItems();
            return _houses.Find(h => h.Id == house.Id).Project(h => mapper.Map<HouseRoot>(h)).FirstOrDefault();
        }

        public async Task<List<RuleEntity>> GetRulesAsync(Guid houseId, Guid sensorId, bool v)
        {
            var filter = Builders<HouseDto>.Filter.And(
                Builders<HouseDto>.Filter.Eq(h => h.Id, houseId),
                Builders<HouseDto>.Filter.ElemMatch(h => h.Sensors, s => s.Id == sensorId)
            );
            var projection = Builders<HouseDto>.Projection.Expression(h =>
                h.Rules.Where(r => r.SensorId == sensorId).ToList());
            List<RuleDto> result = await _houses.Find(filter).Project(projection).FirstOrDefaultAsync();
            return mapper.Map<List<RuleEntity>>(result);
        }
    }
}
