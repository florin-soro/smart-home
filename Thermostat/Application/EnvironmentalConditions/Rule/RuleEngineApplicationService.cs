using Thermostat.Application.EnvironmentalConditions.House.Actions;
using Thermostat.Application.EnvironmentalConditions.House.Services.Interfaces;
using Thermostat.Application.EnvironmentalConditions.Rule.Services.Dto;
using Thermostat.Application.EnvironmentalConditions.Rule.Services.Interfaces;
using Thermostat.Domain.Domain.House.Repositories;
using Thermostat.Domain.Domain.HouseAgg.Rule;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext;

namespace Thermostat.Application.EnvironmentalConditions.Rule
{
    public class RuleEngineApplicationService : IRuleEngineApplicationService
    {
        private readonly ActionExecutorRegistry _executorRegistry;
        private readonly IHouseQueryRepository houseRepository;

        private readonly ISensorMeasurementService sensorMeasurementService;
        private readonly IRuleContextFactory ruleContextFactory;

        public RuleEngineApplicationService(ActionExecutorRegistry executorRegistry, IHouseQueryRepository houseRepository, ISensorMeasurementService sensorMeasurementService, IRuleContextFactory ruleContextFactory)
        {
            _executorRegistry = executorRegistry;
            this.houseRepository = houseRepository;
            this.sensorMeasurementService = sensorMeasurementService;
            this.ruleContextFactory = ruleContextFactory;
        }

        private async Task ExecuteRuleAsync(RuleEntity rule, IRuleContext context, double value)//todo remove or use value
        {
            if (await rule.EvaluateAsync(context))
            {
                var executor = _executorRegistry.Resolve(rule.Action);
                await executor.ExecuteAsync(rule.Action);//ok for now but use events for side effects.
            }
        }

        public async Task ProcessAsync(Guid houseId, Guid sensorId,NewMeasurementDto  measurement)
        {
            var lastMeasurement = await sensorMeasurementService.GetPreviousMeasurementsAsync(houseId, sensorId, measurement.Timestamp);
            //todo detect outlier -> implement a service
            var rules = await houseRepository.GetRulesAsync(houseId, sensorId, true);

            foreach (var rule in rules)
            {
                var context = ruleContextFactory.Create(houseId, rule.Sensor.Id);

                await ExecuteRuleAsync(rule,context, measurement.value);
            }
        }
    }
}
