using Thermostat.Domain.Common.Exceptions;

namespace Thermostat.Domain.Domain.HouseAgg.Exceptions
{
    public class InvalidHouseException : DomainException
    {
        public InvalidHouseException(string message) : base(message) { }
    }
}
