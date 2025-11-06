using System.Diagnostics;
using Thermostat.Application.EnvironmentalConditions.Rule.RuleDefinition.Factory;
using Thermostat.Application.EnvironmentalConditions.Rule.Services.Dto;
using Thermostat.Application.EnvironmentalConditions.Rule.Services.Interfaces;
using Thermostat.DataAccessLayer.Data.Infrastructure.HeatingEnvironmentalConditions.Actions.Factories;
using Thermostat.Domain.Domain.HouseAgg.Repositories;
using Thermostat.Domain.Domain.HouseAgg.Rule;

namespace Thermostat.Application.EnvironmentalConditions.Rule.Services
{
    public class RuleService : IRuleService
    {
        private readonly IHousePersistRepository houseRepository;

        public RuleService(IHousePersistRepository ruleRepository)
        {
            houseRepository = ruleRepository;
        }

        public async Task AddRuleAsync(RuleDto ruleDto)
        {
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }

            if (ruleDto == null)
                throw new InvalidProgramException(nameof(ruleDto));

            var house = await houseRepository.GetHouseAsync(ruleDto.HouseId);
            var sensor = house.GetSensor(ruleDto.SensorId);
            var rule = new RuleEntity(
                Guid.Empty,
                sensor,
                new RuleNameVO(ruleDto.Name),
                new RuleDefinitionAbstractFactory().Create(ruleDto.RuleName,
                    ruleDto.RuleParameters
                ),
                new ActionDefinitionAbstractFactory().Create(ruleDto.Action,
                ruleDto.ActionParameters),
                false
            );
            house.AddRule(rule);

            rule.Enable();
            _ = await houseRepository.UpdateAsync(house);
        }
    }
}
