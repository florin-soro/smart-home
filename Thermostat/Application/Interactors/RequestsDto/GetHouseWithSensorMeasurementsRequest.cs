namespace Thermostat.Application.Interactors.RequetsDto
{
    public class GetHouseWithSensorMeasurementsRequest
    {
        public Guid HouseId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
