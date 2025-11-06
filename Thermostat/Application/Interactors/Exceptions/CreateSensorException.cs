namespace Thermostat.Application.Interactors.Excetions
{
    public class CreateSensorException : ApplicationException
    {
        public CreateSensorException()
        {
        }
        public CreateSensorException(string message) : base(message)
        {
        }
        public CreateSensorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
