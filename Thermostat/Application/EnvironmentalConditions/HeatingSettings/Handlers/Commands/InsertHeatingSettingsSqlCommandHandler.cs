using MediatR;
using Thermostat.Application.Common.Exceptions;
using Thermostat.Domain.Domain.HeatingSettingsAgg;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Commands.InsertHeatingSettingsCommand;
using Thermostat.Domain.Domain.HeatingSettingsAgg.Repositories;

namespace Thermostat.Application.EnvironmentalConditions.HeatingSettings.Handlers.Commands
{
    public class InsertHeatingSettingsSqlCommandHandler : IRequestHandler<InsertHeatingSettingsCommand>
    {
        private readonly IHeatingSettingsRepository heatingSettingsRepository;

        public InsertHeatingSettingsSqlCommandHandler(IHeatingSettingsRepository heatingSettingsRepository)
        {
            this.heatingSettingsRepository = heatingSettingsRepository;
        }

        public async Task Handle(InsertHeatingSettingsCommand request, CancellationToken cancellationToken)
        {
            await InsertHeatingSettingsAsync(request.HeatingSettings);
        }

        public async Task InsertHeatingSettingsAsync(HeatingSettingsRoot heatingSettings)
        {
            if (heatingSettings == null)
            {
                throw new InvalidlParameterException(nameof(heatingSettings));
            }

            await heatingSettingsRepository.AddAsync(heatingSettings);
        }
    }
}
