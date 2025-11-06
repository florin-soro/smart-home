using Thermostat.Domain.Common.Exceptions;

namespace Thermostat.Domain.Domain.HouseAgg.Exceptions
{
    public class SensorPersistenceException : DomainException
    {
        public SensorPersistenceException(string message) : base(message) { }
    }
}
