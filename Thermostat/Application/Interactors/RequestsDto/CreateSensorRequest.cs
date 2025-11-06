namespace Thermostat.Application.Interactors.RequetsDto
{
    public class CreateSensorRequest
    {
        public Guid HouseId { get; set; }
        public string Type { get; set; }
        public string Unit { get; set; }
        public string RoomName { get; set; }
        public double RoomArea { get; set; }
        public string RoomType { get; set; }
    }
}
