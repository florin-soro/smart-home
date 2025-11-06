using MediatR;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Queries.GetTemperatureLowQuery;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories;

namespace Thermostat.Application.EnvironmentalConditions.HeatingSettings.Handlers.Queries
{
    public class GetTemperatureLowSqlQueryHandler : IRequestHandler<GetTemperatureLowQuery, double>
    {
        private readonly IHeatingSettingsReadRepository repository;

        public GetTemperatureLowSqlQueryHandler(IHeatingSettingsReadRepository repository)
        {
            this.repository = repository;
        }

        public async Task<double> Handle(GetTemperatureLowQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetTemperatureLowAsync(request.At);
        }
    }
}
