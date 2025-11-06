using Thermostat.Domain.Domain.HouseAgg.Rule;

namespace Thermostat.DataAccessLayer.Data.Infrastructure.HeatingEnvironmentalConditions.Actions.Factories
{
    public interface IActionDefinitionFactory<T> where T : ActionDefinitionVO
    {
        T Create(Dictionary<string, string> parameters);
    }
}
