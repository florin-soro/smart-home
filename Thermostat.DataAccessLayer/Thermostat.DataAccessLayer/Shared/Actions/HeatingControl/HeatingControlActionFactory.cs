using Thermostat.Application.EnvironmentalConditions.Rule.Action;

namespace Thermostat.DataAccessLayer.Shared.Actions.HeatingControl
{
    public class HeatingControlActionFactory : IHeatingControlActionFactory
    {
        public HeatingControlAction CreateStartHeatingAction()
        {
            return new HeatingControlAction("StartHeating");
        }

        public HeatingControlAction CreateStopHeatingAction()
        {
            return new HeatingControlAction("StopHeating");
        }

        public HeatingControlAction CreateCustomAction(string name)
        {
            return new HeatingControlAction(name);
        }
    }
}
