using System.Data;
using Thermostat.Domain.Common;
using Thermostat.Domain.Common.Exceptions;
using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg.Room;
using Thermostat.Domain.Domain.HouseAgg.Rule;
using Thermostat.Domain.Domain.HouseAgg.Sensor;
using Thermostat.Domain.Domain.Messages.Events;
using Thermostat.Domain.Shared;
using static Thermostat.Domain.Shared.Guard;

namespace Thermostat.Domain.Domain.HouseAgg
{
    public class HouseRoot : AggregateRoot
    {
        private readonly List<SensorEntity> sensors;
        private readonly List<RuleEntity> rules;
        private readonly List<SensorEntity> _newSensors = new();
        private readonly List<RuleEntity> _newRules = new();
        public IReadOnlyCollection<SensorEntity> Sensors => sensors.AsReadOnly();
        public IReadOnlyCollection<RuleEntity> Rules => rules.AsReadOnly();
        internal IReadOnlyCollection<Sensor.SensorEntity> NewSensors => _newSensors.AsReadOnly();
        internal IReadOnlyCollection<RuleEntity> NewRules => _newRules.AsReadOnly();

        public string Name { get; private set; }
        public bool HasNewSensorsAdded => _newSensors.Any();
        public bool HasNewRulesAdded => _newRules.Any();

        public bool HasNewMeasurementsAdded => sensors.Any(s => s.HasNewMeasurements);

        protected HouseRoot() : base()
        {
            sensors = new List<SensorEntity>();
            rules = new List<RuleEntity>();
            Name = string.Empty; 
            PushCreationEvent();
        }
        public HouseRoot(Guid id,string name, List<SensorEntity>? sensors = null, List<RuleEntity>? ruleEntities=null) : base(id)
        {
            Id = Validate(id, nameof(id), IsGuidNotEmpty());
            Name = CheckMultiple(name, $"{nameof(HouseRoot)}.{nameof(Name)}", IsStringNotNullOrEmpty, (n, p) => n.Length > 3 && n.Length <= 100);
            this.sensors = sensors ?? new();
            rules = ruleEntities ?? new();

            PushCreationEvent();
        }

        private void PushCreationEvent()
        {
            var creationEvent = new AggregateOperationDoneEvent(Id, nameof(HouseRoot), this, DateTime.UtcNow);
            AddDomainEvent(creationEvent);
        }
        public IReadOnlyCollection<SensorEntity> GetSensors(string roomName)
        {
            var rooms = sensors.Where(x => x.Room.Name == roomName).ToList();
            if (rooms.Count == 0)
            {
                throw new InvalidOperationException("Room not found.");
            }

            return rooms.AsReadOnly();
        }
        public IReadOnlyCollection<SensorEntity> GetSensors(RoomType roomType)
        {
            var rooms = sensors.Where(x => x.Room.RoomType == roomType).ToList();
            if (rooms.Count == 0)
            {
                throw new InvalidOperationException("Room not found.");
            }

            return rooms.AsReadOnly();
        }
        public SensorEntity GetSensor(Guid sensorId)
        {
            var sensor = sensors.SingleOrDefault(s => s.Id == sensorId);
            if (sensor is null)
            {
                throw new EntityNotFoundException($"Sensor with ID {sensorId} not found.");
            }
            return sensor;
        }
        public SensorEntity GetSensorByRoom(string roomName)
        {
            var room = sensors.SingleOrDefault(s => s.Room.Name == roomName);
            if (room is null) // Fix for CS8604/CS8625
            {
                throw new EntityNotFoundException($"Room with name {roomName} not found.");
            }
            return room;
        }

        public void AddSensor(SensorEntity sensor)
        {
            var validSensor = NotNull(sensor, nameof(sensor));

            if (sensors.Any(s => s.Id == validSensor.Id))
                throw new InvalidOperationException("Sensor already exists in the collection.");

            sensors.Add(validSensor);
            _newSensors.Add(validSensor);

            var creationEvent = new AggregateOperationDoneEvent(Id, nameof(AddSensor), sensor, DateTime.UtcNow);

            AddDomainEvent(creationEvent);
        }

        public void RemoveSensor(Guid sensorId)
        {
            var sensor = sensors.SingleOrDefault(s => s.Id == sensorId);
            if (sensor is null)
            {
                throw new EntityNotFoundException($"Sensor with ID {sensorId} not found.");
            }
            sensors.Remove(sensor);
        }
        public bool ContainSensor(Guid sensorId)
        {
            return sensors.Any(s => s.Id == sensorId);
        }
        public bool AddMeasurementToSensor(Guid sensorId, SensorMeasurementVO sensorMeasurement)
        {
            var sensor = GetSensor(GuidNotEmpty(sensorId, nameof(sensorId)));

            if (!sensor.AddMeasurement(sensorMeasurement))
            {
                return false;
            }

            var creationEvent = new MeasurementCreatedEvent(Id, sensorId, sensorMeasurement, DateTime.UtcNow);
            AddDomainEvent(creationEvent);

            return true;
        }
        public void AddRule(RuleEntity rule)
        {
            var DoesSensorExist = new ValidationRule<RuleEntity>(
                rule => sensors.Any(s => s.Id == rule.Sensor.Id),
                "Sensor referenced in rule does not exist in house.");

            var validRule = Validate(rule, nameof(rule),
                IsNotNull<RuleEntity>(),
                DoesSensorExist);

            if (rules.Contains(validRule))
            {
                throw new DomainRuleValidationException("Rule already exists in the collection.");
            }

            rules.Add(validRule);
            _newRules.Add(validRule);

            var successEvent = new AggregateOperationDoneEvent(Id, nameof(AddRule), rule, DateTime.UtcNow);
            AddDomainEvent(successEvent);
        }

        public void ClearNewItems()
        {
            sensors.ForEach(s => s.ClearNewMeasurements());
            _newRules.Clear();
            _newSensors.Clear();
        }
    }
}
