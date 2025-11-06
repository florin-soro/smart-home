namespace Thermostat.Application.Interactors.RequetsDto
{
    public class CreateHouseRequest
    {
        public List<CreateSensorRequest> Sensors { get; init; }
        public string Name { get; set; }
    }
}
