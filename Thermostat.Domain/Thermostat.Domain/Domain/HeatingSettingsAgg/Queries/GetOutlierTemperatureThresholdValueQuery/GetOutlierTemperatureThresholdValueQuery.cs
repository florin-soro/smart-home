using MediatR;

namespace Thermostat.Domain.Domain.HeatingSettingsAgg.Queries.GetOutlierTemperatureThresholdValueQuery
{
    public class GetOutlierTemperatureThresholdValueQuery: IRequest<double>
    {
        public DateTime At { get; set; }
    }
}
