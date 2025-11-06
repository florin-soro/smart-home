using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thermostat.Application.Interactors;
using Thermostat.Application.Interactors.RequetsDto;

namespace Thermostat.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public partial class HouseController : ControllerBase
    {
        private readonly CreateHouseInteractor createHouseInteractor;
        private readonly GetHouseWithSensorMeasurementsInteractor getHouseWithSensorMeasurementsInteractor;
        private readonly AddSensorInteractor addSensorInteractor;

        public HouseController(CreateHouseInteractor createHouseInteractor, GetHouseWithSensorMeasurementsInteractor getSensorMeasurementsInteractor, AddSensorInteractor addSensorInteractor)
        {
            this.createHouseInteractor = createHouseInteractor;
            this.getHouseWithSensorMeasurementsInteractor = getSensorMeasurementsInteractor;
            this.addSensorInteractor = addSensorInteractor;
        }

        [HttpPost("/create")]
        public async Task<IActionResult> CreateHouse([FromBody] CreateHouseRequest command)
        {
            var id = await createHouseInteractor.HandleAsync(command);
            return CreatedAtAction(nameof(CreateHouse), new { id }, null);//todo understand this line of code
        }

        [HttpGet("{houseId}")]
        public async Task<IActionResult> GetHouseWithSensorMeasurements(Guid houseId, [FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var house = await getHouseWithSensorMeasurementsInteractor.HandleAsync(new GetHouseWithSensorMeasurementsRequest
            {
                HouseId = houseId,
                Start = start,
                End = end
            });
            return Ok(house);
        }
        [HttpPost("{houseId}/sensors/create")]
        public async Task<IActionResult> AddSensor([FromRoute] Guid houseId, [FromBody] CreateSensorDto sensorDto)
        {
            var sensorId = await addSensorInteractor.HandleAsync(
                new CreateSensorRequest
                {
                    HouseId = houseId,
                    Type = sensorDto.Type,
                    Unit = sensorDto.Unit,
                    RoomName = sensorDto.RoomName,
                    RoomArea = sensorDto.RoomArea,
                    RoomType = sensorDto.RoomType
                });
            return CreatedAtAction(nameof(AddSensor), new { sensorId, sensorType = sensorDto.Type }, null);
        }
    }
}