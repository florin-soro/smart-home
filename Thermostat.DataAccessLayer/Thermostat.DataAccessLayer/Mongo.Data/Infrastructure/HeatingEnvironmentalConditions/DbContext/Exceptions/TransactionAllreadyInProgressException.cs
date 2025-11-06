using Thermostat.DataAccessLayer.Exceptions;

namespace Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.DbContext.Exceptions
{
    public class TransactionAllreadyInProgressException : InfrastructureException
    {
        public TransactionAllreadyInProgressException(string message) : base(message)
        {
        }
        public TransactionAllreadyInProgressException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public TransactionAllreadyInProgressException() : base("Transaction is already in progress.")
        {
        }
    }
}
