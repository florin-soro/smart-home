using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Thermostat.Api.Controllers.Dto;
using Thermostat.Application.Interactors;
using Thermostat.Application.Interactors.RequetsDto;

namespace Thermostat.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ThermostatController : ControllerBase
    {
        private readonly ILogger<ThermostatController> _logger;
        private readonly InsertSensorMeasurementInteractor createSensorMeasurementInteractor;
        private readonly InsertHeatingSettingInteractor insertHeatingSettingInteractor;
        public ThermostatController(ILogger<ThermostatController> logger)
        {
            _logger = logger;
        }

        [HttpPost("{houseId}/{sensorId}/measurement/create")]
        public async Task<IActionResult> Post([FromRoute, SwaggerParameter("House Id", Required = true, Description = "49c360a7-9ac6-454d-8885-cbb4b211cd9c")] Guid houseId,
        [FromRoute, SwaggerParameter("Sensor Id", Required = true, Description = "d13a1340-e92a-475e-a6a5-e60e7f9376ad")] Guid sensorId, [FromBody] EnvConditionsMeasurementDto data)
        {
            if (data == null)
            {
                return BadRequest("Invalid data.");
            }

            await createSensorMeasurementInteractor.HandleAsync(new InsertSensorMeasurementRequest
            {
                HouseId = houseId,
                SensorId = sensorId,
                Timestamp = data.Timestamp,
                Value = data.Value
            });

            return Ok();
        }

        [HttpPost("heating-settings")]
        public async Task<IActionResult> SaveHeatingSettings([FromBody] HeatingSettingsDto settings)
        {
            if (settings == null)
            {
                return BadRequest("Invalid settings.");
            }

            await insertHeatingSettingInteractor.HandleAsync(new InsertHeatingSettingsRequest
            {
                Timestamp = settings.Timestamp,
                TemperatureHigh = settings.TemperatureHigh,
                TemperatureLow = settings.TemperatureLow,
                HumidityAlertThreshold = settings.HumidityAlertThreshold,

            });

            _logger.LogInformation("Heating settings saved: {@Settings}", settings);
            return Ok("Heating settings saved successfully.");
        }
    }
}