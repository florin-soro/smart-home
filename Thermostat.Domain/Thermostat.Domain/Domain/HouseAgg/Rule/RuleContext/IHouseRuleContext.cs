using Thermostat.Domain.Domain.House.Sensor;

namespace Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext
{
    public interface IHouseRuleContext : IRuleContext
    {
        Task<SensorMeasurementVO?> GetLastValueStartingFromAsync(DateTime timestamp);
        Task<IEnumerable<SensorMeasurementVO>> GetSensorValuesAsync(DateTime start, DateTime end);
    }
}
