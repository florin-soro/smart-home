using Thermostat.Application.EnvironmentalConditions.HeatingSettings.Services.Interfaces;
using Thermostat.Application.Interactors.Excetions;
using Thermostat.Application.Interactors.RequetsDto;
namespace Thermostat.Application.Interactors
{
    public class InsertHeatingSettingInteractor
    {
        private readonly IHeatingSettingsWriter heatingSettingsWriter;
        public InsertHeatingSettingInteractor(IHeatingSettingsWriter heatingSettingsWriter)
        {
            this.heatingSettingsWriter = heatingSettingsWriter;
        }
        public async Task HandleAsync(InsertHeatingSettingsRequest insertHeatingSettingRequest)
        {
            try
            {
                await heatingSettingsWriter.WriteHeatingSettingsAsync(insertHeatingSettingRequest.Timestamp, insertHeatingSettingRequest.TemperatureHigh, insertHeatingSettingRequest.TemperatureLow, insertHeatingSettingRequest.HumidityAlertThreshold);
            }
            catch (Exception ex)
            {
                throw new CreateHeatingSettingsException($"Error inserting heating setting: {ex.Message}", ex);
            }
        }
    }
}
