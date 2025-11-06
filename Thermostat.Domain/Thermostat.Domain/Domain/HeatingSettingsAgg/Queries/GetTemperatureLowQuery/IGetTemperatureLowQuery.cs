using MediatR;

namespace Thermostat.Domain.Domain.HeatingSettingsAgg.Queries.GetTemperatureLowQuery
{
    public class GetTemperatureLowQuery: IRequest<double>
    {
        public DateTime At { get; set; }
    }
}
