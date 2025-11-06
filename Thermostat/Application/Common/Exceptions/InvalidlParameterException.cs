namespace Thermostat.Application.Common.Exceptions
{
    public class InvalidlParameterException : ApplicationException
    {
        public InvalidlParameterException(string parameterName)
            : base($"The parameter '{parameterName}' is not valid")
        {
        }
        public InvalidlParameterException(string parameterName, Exception innerException)
            : base($"The parameter '{parameterName}' is not valid.", innerException)
        {
        }
    }
}
