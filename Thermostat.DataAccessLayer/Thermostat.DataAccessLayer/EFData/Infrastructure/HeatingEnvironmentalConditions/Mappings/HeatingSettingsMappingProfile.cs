using AutoMapper;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;
using Thermostat.Domain.Domain.HeatingSettingsAgg;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Mappings
{
    public class HeatingSettingsMappingProfile : Profile
    {
        public HeatingSettingsMappingProfile()
        {
            CreateMap<HeatingSettingsRoot, HeatingSettingsEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp))
                .ForMember(dest => dest.TempLow, opt => opt.MapFrom(src => src.TemperatureRange.MinTemperature))
                .ForMember(dest => dest.TempHigh, opt => opt.MapFrom(src => src.TemperatureRange.MaxTemperature))
                .ForMember(dest => dest.HumidityAlertThreshold, opt => opt.MapFrom(src => src.HumidityAlertThreshold));

            CreateMap<HeatingSettingsEntity, HeatingSettingsRoot>()
                .ConstructUsing(entity => new HeatingSettingsRoot(
                    entity.Id,
                    entity.Timestamp,
                    new TemperatureRangeVO(entity.TempLow, entity.TempHigh),
                    entity.HumidityAlertThreshold
                ));
        }
    }
}
