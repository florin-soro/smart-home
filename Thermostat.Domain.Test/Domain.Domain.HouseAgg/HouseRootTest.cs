using Thermostat.Domain.Common.Exceptions;
using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg;
using Thermostat.Domain.Domain.HouseAgg.Room;
using Thermostat.Domain.Domain.HouseAgg.Rule;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;
using Thermostat.Domain.Domain.HouseAgg.Sensor;
using Moq;

namespace Thermostat.Domain.Test.Domain.Domain.HouseAgg;

public class HouseRootTests
{
    private const string ValidName = "MyHouse";
    private const string InvalidEmptyName = "";
    private const string InvalidShortName = "ab";
    private readonly string InvalidLongName = new string('x', 101);

    private static readonly SensorTypeVO ValidSensorType = new("Temperature");
    private static readonly SensorUnitVO ValidSensorUnit = new("°C");
    private static readonly RoomVO ValidRoom = new("Bedroom", 15.5, RoomType.Bedroom);
    private static readonly RoomVO ValidKitchenRoom = new("Kitchen", 20.0, RoomType.Kitchen);

    private static readonly RuleNameVO ValidRuleName = new("TurnOnWhenCold");
    private static readonly ActionDefinitionVO ValidAction = new TestActionDefinitionVO("TurnOn");
    private static readonly IRuleDefinitionVO ValidRuleDefinition = new BetweenRuleDefinition(20,22); // Always satisfied

    internal sealed record TestActionDefinitionVO : ActionDefinitionVO
    {
        public override IReadOnlyDictionary<string, string> Parameters { get; }

        public TestActionDefinitionVO(string type, params (string key, string value)[] parameters)
            : base(type)
        {
            Parameters = parameters.ToDictionary(p => p.key, p => p.value);
        }

        // Optional: constructor with empty parameters
        public TestActionDefinitionVO(string type) : this(type, Array.Empty<(string, string)>()) { }
    }

    [Fact]
    public void GivenEmptyId_WhenCreatingHouse_ThenThrowsDomainRuleValidationException()
    {
        // Arrange
        var id = Guid.Empty;

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new HouseRoot(id, ValidName));

        Assert.Contains($"Guid must not be empty.", exception.Message);
    }

    [Fact]
    public void GivenNullName_WhenCreatingHouse_ThenThrowsDomainRuleValidationException()
    {
        // Arrange
        var nullName = (string)null;

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new HouseRoot(Guid.NewGuid(), nullName));

        Assert.Contains(nameof(HouseRoot.Name), exception.Message);
    }

    [Fact]
    public void GivenEmptyName_WhenCreatingHouse_ThenThrowsDomainRuleValidationException()
    {
        // Arrange
        var emptyName = "";

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new HouseRoot(Guid.NewGuid(), emptyName));

        Assert.Contains(nameof(HouseRoot.Name), exception.Message);
    }

    [Fact]
    public void GivenNameTooShort_WhenCreatingHouse_ThenThrowsDomainRuleValidationException()
    {
        // Arrange
        var shortName = "ab";

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new HouseRoot(Guid.NewGuid(), shortName));

        Assert.Contains(nameof(HouseRoot.Name), exception.Message);
    }

    [Fact]
    public void GivenNameTooLong_WhenCreatingHouse_ThenThrowsDomainRuleValidationException()
    {
        // Arrange
        var longName = new string('x', 101);

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new HouseRoot(Guid.NewGuid(), longName));

        Assert.Contains(nameof(HouseRoot.Name), exception.Message);
    }

    [Fact]
    public void GivenValidArguments_WhenCreatingHouse_ThenCreatesWithCorrectState()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = ValidName;

        // Act
        var house = new HouseRoot(id, name);

        // Assert
        Assert.Equal(id, house.Id);
        Assert.Equal(name, house.Name);
        Assert.Empty(house.Sensors);
        Assert.Empty(house.Rules);
        Assert.False(house.HasNewSensorsAdded); // Indirectly observable
        Assert.False(house.HasNewRulesAdded);   // Indirectly observable
        Assert.False(house.HasNewMeasurementsAdded);
    }

    [Fact]
    public void GivenSensorsAndRules_WhenCreatingHouse_ThenTheyAreAssigned()
    {
        // Arrange
        var id = Guid.NewGuid();

        var sensor1 = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidRoom);
        var sensor2 = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidKitchenRoom);

        var rule1 = new RuleEntity(sensor1, ValidRuleName, ValidRuleDefinition, ValidAction);
        var rule2 = new RuleEntity(sensor2, ValidRuleName, ValidRuleDefinition, ValidAction);

        var sensors = new List<SensorEntity> { sensor1, sensor2 };
        var rules = new List<RuleEntity> { rule1, rule2 };

        // Act
        var house = new HouseRoot(id, ValidName, sensors, rules);

        // Assert
        Assert.Equal(2, house.Sensors.Count());
        Assert.Equal(2, house.Rules.Count());
        Assert.Same(sensor1, house.Sensors.First());
        Assert.Same(rule1, house.Rules.First());
    }

    [Fact]
    public void GivenNullSensors_WhenCreatingHouse_ThenUsesEmptyList()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var house = new HouseRoot(id, ValidName, null, null);

        // Assert
        Assert.Empty(house.Sensors);
        Assert.Empty(house.Rules);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GivenInvalidRoomName_WhenGettingSensorsByRoom_ThenThrowsInvalidOperationException(string roomName)
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => house.GetSensors(roomName));

        Assert.Equal("Room not found.", exception.Message);
    }

    [Fact]
    public void GivenValidRoomName_WhenGettingSensorsByRoom_ThenReturnsMatchingSensors()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor1 = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, new RoomVO("Bedroom", 12.0, RoomType.Bedroom));
        var sensor2 = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, new RoomVO("Bedroom", 14.0, RoomType.Bedroom));
        var sensor3 = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, new RoomVO("Kitchen", 18.0, RoomType.Kitchen));

        house.AddSensor(sensor1);
        house.AddSensor(sensor2);
        house.AddSensor(sensor3);

        // Act
        var result = house.GetSensors("Bedroom");

        // Assert
        Assert.Collection(result,
            s => Assert.Same(sensor1, s),
            s => Assert.Same(sensor2, s));
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GivenRoomType_WhenGettingSensorsByRoomType_ThenReturnsMatchingSensors()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor1 = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, new RoomVO("Bedroom", 12.0, RoomType.Bedroom));
        var sensor2 = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, new RoomVO("Study", 10.0, RoomType.Bedroom));
        var sensor3 = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, new RoomVO("Kitchen", 18.0, RoomType.Kitchen));

        house.AddSensor(sensor1);
        house.AddSensor(sensor2);
        house.AddSensor(sensor3);

        // Act
        var result = house.GetSensors(RoomType.Bedroom);

        // Assert
        Assert.Collection(result,
            s => Assert.Same(sensor1, s),
            s => Assert.Same(sensor2, s));
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GivenNonExistentRoomType_WhenGettingSensorsByRoomType_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => house.GetSensors(RoomType.Bedroom));

        Assert.Equal("Room not found.", exception.Message);
    }

    [Fact]
    public void GivenValidSensorId_WhenGettingSensor_ThenReturnsSensor()
    {
        // Arrange
        var sensorId = Guid.NewGuid();
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(sensorId, ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        // Act
        var result = house.GetSensor(sensorId);

        // Assert
        Assert.Same(sensor, result);
    }

    [Fact]
    public void GivenNonExistentSensorId_WhenGettingSensor_ThenThrowsEntityNotFoundException()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<EntityNotFoundException>(
            () => house.GetSensor(nonExistentId));

        Assert.Equal($"Sensor with ID {nonExistentId} not found.", exception.Message);
    }

    [Fact]
    public void GivenValidRoomName_WhenGettingSensorByRoom_ThenReturnsFirstMatchingSensor()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        // Act
        var result = house.GetSensorByRoom("Bedroom");

        // Assert
        Assert.Same(sensor, result);
    }

    [Fact]
    public void GivenNonExistentRoomName_WhenGettingSensorByRoom_ThenThrowsEntityNotFoundException()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);

        // Act & Assert
        var exception = Assert.Throws<EntityNotFoundException>(
            () => house.GetSensorByRoom("NonExistentRoom"));

        Assert.Equal("Room with name NonExistentRoom not found.", exception.Message);
    }

    [Fact]
    public void GivenNullSensor_WhenAddingSensor_ThenThrowsArgumentNullException()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => house.AddSensor(null!));

        Assert.Equal("sensor cannot be null.", exception.Message);
    }

    [Fact]
    public void GivenSensorWithExistingId_WhenAddingSensor_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var sensorId = Guid.NewGuid();
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor1 = new SensorEntity(sensorId, ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor1);

        var sensor2 = new SensorEntity(sensorId, ValidSensorType, ValidSensorUnit, ValidKitchenRoom); 

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => house.AddSensor(sensor2));

        Assert.Equal("Sensor already exists in the collection.", exception.Message);
    }

    [Fact]
    public void GivenValidSensor_WhenAddingSensor_ThenHasNewSensorsAddedBecomesTrue()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidRoom);

        // Act
        house.AddSensor(sensor);

        // Assert
        Assert.Single(house.Sensors);
        Assert.Same(sensor, house.Sensors.First());
        Assert.True(house.HasNewSensorsAdded); 
    }

    [Fact]
    public void GivenTwoSensorsAdded_WhenAddingSecond_ThenHasNewSensorsAddedIsTrue()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor1 = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidRoom);
        var sensor2 = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidRoom);

        // Act
        house.AddSensor(sensor1);
        house.AddSensor(sensor2);

        // Assert
        Assert.Equal(2, house.Sensors.Count());
        Assert.True(house.HasNewSensorsAdded);
    }

    [Fact]
    public void GivenSensorAdded_WhenRemovingSensor_ThenHasNewSensorsAddedRemainsTrue()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        // Act
        house.RemoveSensor(sensor.Id);

        // Assert
        Assert.Empty(house.Sensors);
        Assert.True(house.HasNewSensorsAdded); 
    }

    [Fact]
    public void GivenSensorAdded_WhenClearingNewItems_ThenHasNewSensorsAddedBecomesFalse()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        // Act
        house.ClearNewItems();

        // Assert
        Assert.True(house.Sensors.Contains(sensor)); 
        Assert.False(house.HasNewSensorsAdded);      
    }

    [Fact]
    public void GivenNoSensorsAdded_WhenClearingNewItems_ThenHasNewSensorsAddedIsFalse()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);

        // Act
        house.ClearNewItems();

        // Assert
        Assert.False(house.HasNewSensorsAdded); 
    }

    [Fact]
    public void GivenNonExistentSensorId_WhenRemovingSensor_ThenThrowsEntityNotFoundException()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<EntityNotFoundException>(
            () => house.RemoveSensor(nonExistentId));

        Assert.Equal($"Sensor with ID {nonExistentId} not found.", exception.Message);
    }

    [Fact]
    public void GivenExistentSensorId_WhenCheckingContainSensor_ThenReturnsTrue()
    {
        // Arrange
        var sensorId = Guid.NewGuid();
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(sensorId, ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        // Act
        var result = house.ContainSensor(sensorId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GivenNonExistentSensorId_WhenCheckingContainSensor_ThenReturnsFalse()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = house.ContainSensor(nonExistentId);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void GivenInvalidSensorId_WhenAddingMeasurement_ThenThrowsDomainRuleValidationException(Guid sensorId)
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var measurement = new SensorMeasurementVO(DateTime.UtcNow, 22.5);

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => house.AddMeasurementToSensor(sensorId, measurement));

        Assert.Contains(nameof(sensorId), exception.Message);
    }

    [Fact]
    public void GivenNonExistentSensorId_WhenAddingMeasurement_ThenThrowsEntityNotFoundException()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var measurement = new SensorMeasurementVO(DateTime.UtcNow, 22.5);
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<EntityNotFoundException>(
            () => house.AddMeasurementToSensor(nonExistentId, measurement));

        Assert.Equal($"Sensor with ID {nonExistentId} not found.", exception.Message);
    }

    [Fact]
    public void GivenExistentSensor_WhenAddingValidMeasurement_ThenHasNewMeasurementsAddedBecomesTrue()
    {
        // Arrange
        var sensorId = Guid.NewGuid();
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(sensorId, ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        var measurement = new SensorMeasurementVO(DateTime.UtcNow, 22.5);

        // Act
        var result = house.AddMeasurementToSensor(sensorId, measurement);

        // Assert
        Assert.True(result);
        Assert.True(house.HasNewMeasurementsAdded);
    }

    [Fact]
    public void GivenNullRule_WhenAddingRule_ThenThrowsArgumentNullException()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => house.AddRule(null!));

        Assert.Contains("Value must not be null.", exception.Message);
    }

    [Fact]
    public void GivenRuleWithNonExistentSensor_WhenAddingRule_ThenThrowsDomainRuleValidationException()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidRoom);
        var rule = new RuleEntity(sensor, ValidRuleName, ValidRuleDefinition, ValidAction);

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => house.AddRule(rule));

        Assert.Contains("Sensor referenced in rule does not exist in house.", exception.Message);
    }

    [Fact]
    public void GivenRuleWithExistingSensor_WhenAddingRule_ThenHasNewRulesAddedBecomesTrue()
    {
        // Arrange
        var sensorId = Guid.NewGuid();
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(sensorId, ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        var rule = new RuleEntity(sensor, ValidRuleName, ValidRuleDefinition, ValidAction);

        // Act
        house.AddRule(rule);

        // Assert
        Assert.Single(house.Rules);
        Assert.Same(rule, house.Rules.First());
        Assert.True(house.HasNewRulesAdded); 
    }

    [Fact]
    public void GivenDuplicateRule_WhenAddingRule_ThenThrowsDomainRuleValidationException()
    {
        // Arrange
        var sensorId = Guid.NewGuid();
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(sensorId, ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        var rule = new RuleEntity(sensor, ValidRuleName, ValidRuleDefinition, ValidAction);
        house.AddRule(rule);

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => house.AddRule(rule));

        Assert.Equal("Rule already exists in the collection.", exception.Message);
    }

    [Fact]
    public void GivenRuleAdded_WhenClearingNewItems_ThenHasNewRulesAddedBecomesFalse()
    {
        // Arrange
        var sensorId = Guid.NewGuid();
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(sensorId, ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        var rule = new RuleEntity(sensor, ValidRuleName, ValidRuleDefinition, ValidAction);
        house.AddRule(rule);

        // Act
        house.ClearNewItems();

        // Assert
        Assert.True(house.Rules.Contains(rule)); 
        Assert.False(house.HasNewRulesAdded);   
    }

    [Fact]
    public void GivenNoRulesAdded_WhenClearingNewItems_ThenHasNewRulesAddedIsFalse()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);

        // Act
        house.ClearNewItems();

        // Assert
        Assert.False(house.HasNewRulesAdded); 
    }

    [Fact]
    public void GivenSensorWithMeasurement_WhenClearingNewItems_ThenHasNewMeasurementsAddedBecomesFalse()
    {
        // Arrange
        var sensorId = Guid.NewGuid();
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(sensorId, ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        var measurement = new SensorMeasurementVO(DateTime.UtcNow, 22.5);
        house.AddMeasurementToSensor(sensorId, measurement);

        // Act
        house.ClearNewItems();

        // Assert
        Assert.False(house.HasNewMeasurementsAdded); 
        Assert.False(sensor.HasNewMeasurements);      
    }

    [Fact]
    public void GivenHouseWithSensors_WhenGettingSensorsByRoom_ThenReturnsReadOnlyCollection()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidRoom);
        house.AddSensor(sensor);

        // Act
        var result = house.GetSensors(RoomType.Bedroom);

        // Assert
        Assert.Null(result as List<SensorEntity>);
        Assert.IsAssignableFrom<IReadOnlyCollection<SensorEntity>>(result);
    }

    [Fact]
    public void GivenHouseWithRules_WhenGettingRules_ThenReturnsReadOnlyCollection()
    {
        // Arrange
        var house = new HouseRoot(Guid.NewGuid(), ValidName);
        var sensor = new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, ValidRoom);
        var rule = new RuleEntity(sensor, ValidRuleName, ValidRuleDefinition, ValidAction);
        house.AddSensor(sensor);
        house.AddRule(rule);

        // Act
        var result = house.Rules;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<RuleEntity>>(result);
        Assert.Null(result as List<RuleEntity>); 
    }

    [Fact]
    public void GivenSensorEntity_WhenConstructedWithNullType_ThenThrowsArgumentNullException()
    {
        // Arrange
        var nullType = (SensorTypeVO)null;

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new SensorEntity(Guid.NewGuid(), nullType, ValidSensorUnit, ValidRoom));

        Assert.Equal("type cannot be null.", exception.Message);
    }

    [Fact]
    public void GivenSensorEntity_WhenConstructedWithNullUnit_ThenThrowsArgumentNullException()
    {
        // Arrange
        var nullUnit = (SensorUnitVO)null;

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new SensorEntity(Guid.NewGuid(), ValidSensorType, nullUnit, ValidRoom));

        Assert.Equal("unit cannot be null.", exception.Message);
    }

    [Fact]
    public void GivenSensorEntity_WhenConstructedWithNullRoom_ThenThrowsArgumentNullException()
    {
        // Arrange
        var nullRoom = (RoomVO)null;

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, nullRoom));

        Assert.Equal("room cannot be null.", exception.Message);
    }

    [Fact]
    public void GivenSensorEntity_WhenConstructedWithEmptyTypeName_ThenThrowsDomainRuleValidationException()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new SensorEntity(Guid.NewGuid(), new SensorTypeVO(""), ValidSensorUnit, ValidRoom));

        Assert.Contains("type cannot be empty or whitespace.", exception.Message);
    }

    [Fact]
    public void GivenSensorEntity_WhenConstructedWithEmptyUnitName_ThenThrowsDomainRuleValidationException()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new SensorEntity(Guid.NewGuid(), ValidSensorType, new SensorUnitVO(""), ValidRoom));

        Assert.Equal("unitName cannot be empty or whitespace.", exception.Message);
    }

    [Fact]
    public void GivenSensorEntity_WhenConstructedWithEmptyRoomName_ThenThrowsDomainRuleValidationException()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, new RoomVO("", 15.5, RoomType.Bedroom)));

        Assert.Contains("name cannot be empty or whitespace", exception.Message);
    }

    [Fact]
    public void GivenSensorEntity_WhenConstructedWithZeroArea_ThenThrowsDomainRuleValidationException()
    {

        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, new RoomVO("Bedroom", 0, RoomType.Bedroom)));

        Assert.Contains("area must be greater than 0", exception.Message);
    }

    [Fact]
    public void GivenSensorEntity_WhenConstructedWithNegativeArea_ThenThrowsDomainRuleValidationException()
    {
        // Act & Assert
        var exception = Assert.Throws<DomainRuleValidationException>(
            () => new SensorEntity(Guid.NewGuid(), ValidSensorType, ValidSensorUnit, new RoomVO("Bedroom", -5.0, RoomType.Bedroom)));

        Assert.Equal("area must be greater than 0.", exception.Message);
    }
}