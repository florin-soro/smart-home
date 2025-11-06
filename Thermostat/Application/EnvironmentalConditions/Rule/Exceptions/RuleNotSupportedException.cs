namespace Thermostat.Application.EnvironmentalConditions.Rule.Exceptions
{
    public class RuleNotSupportedException : ApplicationException
    {
        public RuleNotSupportedException(string message) : base(message)
        {
        }
        public RuleNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
