using AutoMapper;
using Thermostat.Application.EnvironmentalConditions.Rule.RuleDefinition.Factory;
using Thermostat.DataAccessLayer.Data.Infrastructure.HeatingEnvironmentalConditions.Actions.Factories;
using Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.EntitiesDto;
using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg;
using Thermostat.Domain.Domain.HouseAgg.Room;
using Thermostat.Domain.Domain.HouseAgg.Rule;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;
using Thermostat.Domain.Domain.HouseAgg.Sensor;
using SensorDto = Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.EntitiesDto.SensorDto;

namespace Thermostat.DataAccessLayer.Mongo.Data.Infrastructure.HeatingEnvironmentalConditions.Mappings
{
    public class HouseMappingMongoProfile : Profile
    {
        public HouseMappingMongoProfile()
        {
            CreateMap<SensorMeasurementVO, SensorMeasurementDto>();
            CreateMap<SensorMeasurementDto,SensorMeasurementVO>()
                .ConstructUsing(sdto=>new SensorMeasurementVO(sdto.Timestamp,sdto.Value));

            CreateMap<SensorEntity, SensorDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.TypeName))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit.UnitName))
                .ForMember(dest => dest.Room, opt => opt.MapFrom(src => src.Room));

            CreateMap<SensorDto, SensorEntity>()
                .ConstructUsing(dto => new SensorEntity(dto.Id, new SensorTypeVO(dto.Type), new SensorUnitVO(dto.Unit),
                    new RoomVO(dto.Room.Name, dto.Room.Area, (RoomType)Enum.Parse(typeof(RoomType), dto.Room.Type)),
                    dto.Measurements.Select(x => new SensorMeasurementVO(x.Timestamp, x.Value)).ToList()));

            CreateMap<RoomDto, RoomVO>()
                .ConstructUsing(dto => new RoomVO(
                    dto.Name,
                    dto.Area,
                    Enum.Parse<RoomType>(dto.Type)
                ));

            CreateMap<RoomVO, RoomDto>()
           .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.RoomType.ToString()));

            CreateMap<HouseRoot, HouseDto>()
                .ForMember(dest => dest.Sensors, opt => opt.MapFrom(src => src.Sensors))
                .ForMember(dest => dest.Rules, opt => opt.MapFrom(src => src.Rules));


            CreateMap<HouseDto, HouseRoot>()
    .ConvertUsing(dto =>
        new HouseRoot(
            dto.Id,
            dto.Name,
            dto.Sensors.Select(s => MapSensor(s)).ToList(),
            dto.Rules.Select(r => new RuleEntity(r.Id,
                dto.Sensors.Select(MapSensor).Single(s => s.Id == r.SensorId),
                new RuleNameVO(r.Name.Value),
                MapRuleDefinition(r.Rule),
                MapActionDefinition(r.Action),
                r.Enabled)
            ).ToList()
        )
    );



            CreateMap<RuleNameDto, RuleNameVO>()
                .ConstructUsing(src => new RuleNameVO(src.Value));

            CreateMap<RuleNameVO, RuleNameDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));

            CreateMap<RuleEntity, RuleDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest => dest.SensorId, opt => opt.MapFrom(src => src.Sensor.Id))
                .ForMember(dest => dest.Rule, opt => opt.MapFrom(src => MapRuleDefinitionDto(src.RuleDefinition)))
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => MapActionDefinitionDto(src.Action)))
                .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled));

            CreateMap<ActionDefinitionDto, ActionDefinitionVO>()
                .ConvertUsing(dto => MapActionDefinition(dto));

            CreateMap<RuleDefinitionDto, IRuleDefinitionVO>()
                .ConvertUsing(dto => MapRuleDefinition(dto));

        }

        private SensorEntity MapSensor(SensorDto sensorDto)
        {
            return new SensorEntity(
                sensorDto.Id,
                new SensorTypeVO(sensorDto.Type),
                new SensorUnitVO(sensorDto.Unit),
                MapRoom(sensorDto.Room)
            );
        }
        private static IRuleDefinitionVO MapRuleDefinition(RuleDefinitionDto dto)
        {
            Enum.TryParse<RuleDefinitionType>(dto.Type, out var ruleDefinitionType);

            return new RuleDefinitionAbstractFactory().Create(ruleDefinitionType, dto.Parameters);
        }

        private static ActionDefinitionVO MapActionDefinition(ActionDefinitionDto dto)
        {
            return new ActionDefinitionAbstractFactory().Create(dto.Type,dto.Parameters);
        }

        private static RuleDefinitionDto MapRuleDefinitionDto(IRuleDefinitionVO ruleDefinition)
        {
            return new RuleDefinitionDto
            {
                Type = ruleDefinition.RuleDefinitionType.ToString(),
                Parameters = new (ruleDefinition.Parameters)
            };
        }

        private static ActionDefinitionDto MapActionDefinitionDto(ActionDefinitionVO action)
        {
            return new ActionDefinitionDto
            {
                Type = action.Type,
                Parameters = new (action.Parameters)
            };
        }
        private RoomVO MapRoom(RoomDto roomDto)
        {
            return new RoomVO(
                roomDto.Name,
                roomDto.Area,
                Enum.Parse<RoomType>(roomDto.Type)
            );
        }
    }
}
