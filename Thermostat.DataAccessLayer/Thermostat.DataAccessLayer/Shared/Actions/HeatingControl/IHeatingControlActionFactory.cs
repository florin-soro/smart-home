using Thermostat.Application.EnvironmentalConditions.Rule.Action;

namespace Thermostat.DataAccessLayer.Shared.Actions.HeatingControl
{
    public interface IHeatingControlActionFactory
    {
        HeatingControlAction CreateCustomAction(string name);
        HeatingControlAction CreateStartHeatingAction();
        HeatingControlAction CreateStopHeatingAction();
    }
}
