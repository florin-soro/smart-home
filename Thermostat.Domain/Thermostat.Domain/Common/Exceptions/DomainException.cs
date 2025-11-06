namespace Thermostat.Domain.Common.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
        // Additional constructors or properties can be added here if needed
    }
}
