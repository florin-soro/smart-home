namespace Thermostat.Application.EnvironmentalConditions.HeatingSettings.Handlers.Commands.Exceptions
{
    public class InvalidHeatingCommandException : ApplicationException
    {
        public InvalidHeatingCommandException(string message) : base(message)
        {
        }
        public InvalidHeatingCommandException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
