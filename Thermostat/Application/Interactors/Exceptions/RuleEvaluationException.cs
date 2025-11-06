namespace Thermostat.Application.Interactors.Excetions
{
    public class RuleEvaluationException : ApplicationException
    {
        public RuleEvaluationException()
        {
        }
        public RuleEvaluationException(string message) : base(message)
        {
        }
        public RuleEvaluationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
