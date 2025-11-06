using Thermostat.Application.EnvironmentalConditions.Rule.Services.Dto;

namespace Thermostat.Application.EnvironmentalConditions.Rule.Services.Interfaces
{
    public interface IRuleService
    {
        Task AddRuleAsync(RuleDto ruleDto);
    }
}