using Thermostat.Domain.Common.Exceptions;

namespace Thermostat.Domain.Domain.HouseAgg.Exceptions
{
    public class MeasurementEarlierOrEqualToLastMeasurementException : DomainException
    {
        public MeasurementEarlierOrEqualToLastMeasurementException(string message) : base(message) { }
    }
    
}
