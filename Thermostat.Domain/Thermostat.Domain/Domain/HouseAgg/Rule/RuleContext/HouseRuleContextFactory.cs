using Thermostat.Domain.Domain.House.Repositories;

namespace Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext
{
    public class HouseRuleContextFactory : IRuleContextFactory
    {
        IHouseQueryRepository _houseRepository;

        public HouseRuleContextFactory(IHouseQueryRepository houseRepository)
        {
            _houseRepository = houseRepository;
        }

        public IRuleContext Create(Guid houseId, Guid sensorId)
        {
            return new HouseRuleContext(_houseRepository,houseId, sensorId);
        }
    }
}
