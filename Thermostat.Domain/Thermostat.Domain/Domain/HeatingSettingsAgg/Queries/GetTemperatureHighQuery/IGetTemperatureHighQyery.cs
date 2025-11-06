using MediatR;

namespace Thermostat.Domain.Domain.HeatingSettingsAgg.Queries.GetTemperatureHighQuery
{
    public class GetTemperatureHighQuery : IRequest<double>
    {
        public DateTime At { get; set; }
    }
}
