using MediatR;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Queries.GetOutlierTemperatureThresholdValueQuery;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories;
namespace Thermostat.Application.EnvironmentalConditions.HeatingSettings.Handlers.Queries
{
    public class GetOutlierTemperatureThresholdValueSqlQueryHandler : IRequestHandler<GetOutlierTemperatureThresholdValueQuery, double>
    {
        private readonly IHeatingSettingsReadRepository repository;

        public GetOutlierTemperatureThresholdValueSqlQueryHandler(IHeatingSettingsReadRepository repository)
        {
            this.repository = repository;
        }

        public async Task<double> Handle(GetOutlierTemperatureThresholdValueQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetOutlierTemperatureThresholdValueAsync(request.At);
        }
    }
}
