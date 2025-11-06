namespace Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext
{
    public interface IRuleContextFactory
    {
        IRuleContext Create(Guid houseId, Guid sensorId);
    }
}
