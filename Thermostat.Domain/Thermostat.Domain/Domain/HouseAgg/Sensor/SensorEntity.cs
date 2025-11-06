using Thermostat.Domain.Common;
using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg.Room;
using static Thermostat.Domain.Shared.Guard;
namespace Thermostat.Domain.Domain.HouseAgg.Sensor
{
    public class SensorEntity : EntityBase
    {
        private readonly HashSet<SensorMeasurementVO> _measurements = new();
        private readonly HashSet<SensorMeasurementVO> _newMeasurements = new();
        public SensorEntity(Guid guid, SensorTypeVO type, SensorUnitVO unit, RoomVO room,List<SensorMeasurementVO> sensorMeasurementVOs=null) : base(guid)
        {
            AssignValidValues(type, unit, room);
            _measurements = sensorMeasurementVOs?.ToHashSet() ?? new();
        }
        public SensorEntity(SensorTypeVO type, SensorUnitVO unit, RoomVO room) : base()
        {
            AssignValidValues(type, unit, room);
        }

        private void AssignValidValues(SensorTypeVO type, SensorUnitVO unit, RoomVO room)
        {
            Type = NotNull(type, nameof(type));
            Unit = NotNull(unit, nameof(unit));
            Room = NotNull(room, nameof(room));
        }
        public IReadOnlyCollection<SensorMeasurementVO> Measurements => _measurements;
        public IReadOnlyCollection<SensorMeasurementVO> NewMeasurements => _newMeasurements;
        public bool HasNewMeasurements => _newMeasurements.Any();
        public SensorTypeVO Type { get; private set; }
        public SensorUnitVO Unit { get; private set; }
        public RoomVO Room { get; private set; }

        internal bool AddMeasurement(SensorMeasurementVO sensorMeasurement)
        {
            if (_measurements.Add(NotNull(sensorMeasurement, nameof(sensorMeasurement))))
            { 
                _newMeasurements.Add(sensorMeasurement);
                return true;
            }
            return false;
        }
        internal void ClearNewMeasurements()
        {
            _newMeasurements.Clear();
        }
    }
}
