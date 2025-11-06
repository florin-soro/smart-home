using Thermostat.Application.EnvironmentalConditions.Rule.Services.Dto;
using Thermostat.Application.EnvironmentalConditions.Rule.Services.Interfaces;
using Thermostat.Application.Interactors.RequetsDto;
namespace Thermostat.Application.Interactors
{
    public class AddRuleInteractor
    {
        private readonly IRuleService _ruleService;

        public AddRuleInteractor(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        public async Task HandleAsyncAsync(InsertRuleRequest newRuleRequest)
        {
            if (newRuleRequest == null)
                throw new ArgumentNullException(nameof(newRuleRequest));

            var ruleDto = new RuleDto
            {
                Name = newRuleRequest.Name,
                HouseId = newRuleRequest.HouseId,
                SensorId = newRuleRequest.SensorId,
                RuleName = newRuleRequest.RuleName,
                RuleParameters = newRuleRequest.RuleParameters,
                Action = newRuleRequest.Action,
                ActionParameters = newRuleRequest.ActionParameters
            };

            await _ruleService.AddRuleAsync(ruleDto);
        }
    }
}
