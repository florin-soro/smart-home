namespace Thermostat.Api.Config
{
    public class ThermostatSettings : IThermostatSettings
    {
        public double TempLow { get; set; }
        public double TempHigh { get; set; }
        public double HumidityThreshold { get; set; }
    }
}
