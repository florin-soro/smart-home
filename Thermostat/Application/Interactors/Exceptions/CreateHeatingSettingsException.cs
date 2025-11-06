namespace Thermostat.Application.Interactors.Excetions
{
    public class CreateHeatingSettingsException : ApplicationException
    {
        public CreateHeatingSettingsException()
        {
        }
        public CreateHeatingSettingsException(string message) : base(message)
        {
        }
        public CreateHeatingSettingsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
