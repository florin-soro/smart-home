using MediatR;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Queries.GetTemperatureHighQuery;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories;

namespace Thermostat.Application.EnvironmentalConditions.HeatingSettings.Handlers.Queries
{
    public class GetTemperatureHighSqlQueryHandler : IRequestHandler<GetTemperatureHighQuery, double>
    {
        private readonly IHeatingSettingsReadRepository repository;

        public GetTemperatureHighSqlQueryHandler(IHeatingSettingsReadRepository repository)
        {
            this.repository = repository;
        }

        public async Task<double> Handle(GetTemperatureHighQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetTemperatureHighAsync(request.At);
        }
    }
}