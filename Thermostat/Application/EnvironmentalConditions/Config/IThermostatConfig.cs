namespace Thermostat.Api.Config
{
    public interface IThermostatSettings
    {
        double TempHigh { get; set; }
        double TempLow { get; set; }
        double HumidityThreshold { get; set; }
    }
}