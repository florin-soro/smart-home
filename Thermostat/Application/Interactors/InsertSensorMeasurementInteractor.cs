using Thermostat.Application.EnvironmentalConditions.House.Services.Interfaces;
using Thermostat.Application.EnvironmentalConditions.Rule.Services.Dto;
using Thermostat.Application.EnvironmentalConditions.Rule.Services.Interfaces;
using Thermostat.Application.Interactors.Excetions;
using Thermostat.Application.Interactors.RequetsDto;
namespace Thermostat.Application.Interactors
{
    public class InsertSensorMeasurementInteractor
    {
        private readonly IRuleEngineApplicationService ruleApplicationService;
        private readonly ISensorMeasurementService sensorMeasurementService;

        public InsertSensorMeasurementInteractor(IRuleEngineApplicationService ruleApplicationService, ISensorMeasurementService sensorMeasurementService)
        {
            this.ruleApplicationService = ruleApplicationService;
            this.sensorMeasurementService = sensorMeasurementService;
        }

        public async Task HandleAsync(InsertSensorMeasurementRequest createSensorMeasurementRequest)
        {
            try
            {
                await sensorMeasurementService.InsertMeasurementAsync(createSensorMeasurementRequest);
            }
            catch (Exception ex)
            {
                throw new CreateSensorException($"Error inserting measurement: {ex.Message}", ex);
            }

            try
            {
                await ruleApplicationService.ProcessAsync(createSensorMeasurementRequest.HouseId, createSensorMeasurementRequest.SensorId, new NewMeasurementDto(createSensorMeasurementRequest.Timestamp, createSensorMeasurementRequest.Value));
            }
            catch (Exception ex)
            {
                throw new RuleEvaluationException($"Error processing rules: {ex.Message}", ex);
            }
        }
    }
}
