namespace Thermostat.Domain.Common.Exceptions
{
    public class DomainRuleValidationException : DomainException
    {
        public DomainRuleValidationException(string message) : base(message) { }

        public DomainRuleValidationException(string message, Exception innerException) : base(message, innerException) { }

    }
}
