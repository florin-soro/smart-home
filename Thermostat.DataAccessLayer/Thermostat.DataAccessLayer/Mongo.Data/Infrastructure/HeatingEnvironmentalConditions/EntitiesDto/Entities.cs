using MongoDB.Bson.Serialization.Attributes;
namespace Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.EntitiesDto
{
    public class HouseDto
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SensorDto> Sensors { get; set; } = new();
        public List<RuleDto> Rules { get; set; } = new();

    }

    public class SensorDto
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Unit { get; set; }

        public List<SensorMeasurementDto> Measurements { get; set; } = new();
        public RoomDto Room { get; set; }
    }

    public class RuleNameDto
    {
        public string Value { get; set; }
    }

        public class RuleDto
    {
        [BsonId]
        public Guid Id { get; set; }
        public RuleNameDto Name { get; set; } = new();
        public Guid SensorId { get; set; }
        public RuleDefinitionDto Rule { get; set; }
        public ActionDefinitionDto Action { get; set; }
        public bool Enabled { get; set; }
    }

    public class RuleDefinitionDto
    {
        public string Type { get; set; } 
        public Dictionary<string, string> Parameters { get; set; }
    }

    public class ActionDefinitionDto
    {
        public string Type { get; set; } 
        public Dictionary<string, string> Parameters { get; set; }
    }


    public class SensorMeasurementDto   
    {
        public DateTime Timestamp { get; set; }

        public double Value { get; set; }
    }

    public class RoomDto
    {
        public string Name { get; set; }
        public double Area { get; set; }

        public string Type { get; set; } 
    }

    public class HeatingSettingsDto
    {
        [BsonId]
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double TempLow { get; set; }
        public double TempHigh { get; set; }
        public double HumidityAlertThreshold { get; set; }
    }

}
