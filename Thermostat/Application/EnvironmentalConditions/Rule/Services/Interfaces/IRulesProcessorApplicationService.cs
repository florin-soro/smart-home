using Thermostat.Application.EnvironmentalConditions.Rule.Services.Dto;

namespace Thermostat.Application.EnvironmentalConditions.Rule.Services.Interfaces
{
    public interface IRuleEngineApplicationService
    {
        Task ProcessAsync(Guid houseId, Guid sensorId, NewMeasurementDto measurement);
    }
}