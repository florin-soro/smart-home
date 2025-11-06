namespace Thermostat.Api.Controllers
{
    public partial class HouseController
    {
        public class CreateSensorDto
        {
            public string Type { get; set; }
            public string Unit { get; set; }
            public string RoomName { get; set; }
            public double RoomArea { get; set; }
            public string RoomType { get; set; }
        }
    }
}