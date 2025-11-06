namespace Thermostat.Application.EnvironmentalConditions.House.Services.Dto
{
    public class SensorDto
    {
        public string Type { get; set; }
        public string Unit { get; set; }
        public string RoomName { get; set; }
        public double RoomArea { get; set; }
        public string RoomType { get; set; }
    }
}
