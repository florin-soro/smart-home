using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Thermostat.Application.EnvironmentalConditions.Rule.Action;
using Thermostat.Application.EnvironmentalConditions.Rule.RuleDefinition.Factory;
using Thermostat.DataAccessLayer.Data.Infrastructure.HeatingEnvironmentalConditions.Actions.Factories;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.DatabaseContext;
using Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Entities;
using Thermostat.DataAccessLayer.Shared.Actions.HeatingControl;
using Thermostat.Domain.Domain.House.Sensor;
using Thermostat.Domain.Domain.HouseAgg;
using Thermostat.Domain.Domain.HouseAgg.Room;
using Thermostat.Domain.Domain.HouseAgg.Rule;
using Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition;
using Thermostat.Domain.Domain.HouseAgg.Sensor;

namespace Thermostat.DataAccessLayer.EFData.Infrastructure.HeatingEnvironmentalConditions.Mappings
{
    public class HouseMappingSqlProfile : Profile
    {
        public HouseMappingSqlProfile()
        {
            #region HouseRoom
            CreateMap<HouseRoot, HouseEntity>()
                    .PreserveReferences()
            .EqualityComparison((src, dest) => src.Id == dest.Id)
            .ForMember(dest => dest.Sensors, opt => opt.MapFrom(src => src.Sensors))
            .AfterMap((src, dest, context) =>
            {
                foreach (var sensor in dest.Sensors)
                {
                    sensor.HouseRootId = src.Id;
                    sensor.Measurements.ForEach(m =>
                    {
                        m.SensorEntityId = sensor.Id;
                        m.HouseRootId = src.Id;
                    });
                }
            })
            .ForMember(dest => dest.Rules, opt =>
            {
                opt.MapFrom(src => src.Rules);
            })
            .AfterMap((src, dest, context) =>
            {
                foreach (var sensor in dest.Rules)
                {
                    sensor.HouseRootId = src.Id;
                }
            })
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<HouseEntity, HouseRoot>()
                .ConstructUsing(dto => new HouseRoot(
                    dto.Id,
                    dto.Name,
                    dto.Sensors.Select(s => MapSensor(s)).ToList(),
                    dto.Rules.Select(r => CreateRuleDefinition(r, dto)).ToList()))
            .ForMember(dest => dest.Sensors, opt => opt.Ignore())
            .ForMember(dest => dest.Rules, opt => opt.Ignore())
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
            #endregion

            #region Sensor
            CreateMap<Domain.Domain.HouseAgg.Sensor.SensorEntity, Entities.SensorEntity>()
                .EqualityComparison((src, dest) => src.Id == dest.Id)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.TypeName))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit.UnitName))
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name))
                .ForMember(dest => dest.RoomArea, opt => opt.MapFrom(src => src.Room.Area))
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.Room.RoomType))
                .ForMember(dest => dest.HouseRootId, opt => opt.Ignore())
                .ForMember(dest => dest.Measurements, opt => opt.Ignore());

            CreateMap<HeatingEnvironmentalConditions.Entities.SensorEntity, Domain.Domain.HouseAgg.Sensor.SensorEntity>()
                .ConstructUsing(dto => new Thermostat.Domain.Domain.HouseAgg.Sensor.SensorEntity(
                    dto.Id,
                    new SensorTypeVO(dto.Type),
                    new SensorUnitVO(dto.Unit),
                    MapRoom(dto.RoomName, dto.RoomArea, dto.RoomType),
                    dto.Measurements.Select(x => new SensorMeasurementVO(x.Timestamp, x.Value)).ToList()
                ))
                .ForMember(dest => dest.Room, opt => opt.Ignore())
                .ForMember(dest => dest.Measurements, opt => opt.Ignore())
                .ForMember(dest => dest.NewMeasurements, opt => opt.Ignore());
            #endregion

            #region RuleEntity(Domain) <-> RuleEntity(EF)

            CreateMap<Domain.Domain.HouseAgg.Rule.RuleEntity, Entities.RuleEntity>()
            .EqualityComparison((src, dest) => src.Id == dest.Id)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.SensorId, opt => opt.MapFrom(src => src.Sensor.Id))
            .ForMember(dest => dest.RuleDefinition, opt => opt.MapFrom(src => src.RuleDefinition))
            .ForMember(dest => dest.ActionDefinition, opt => opt.MapFrom(src => src.Action))
            .ForMember(dest => dest.HouseRootId, opt => opt.Ignore())
             .AfterMap((src, dest, context) =>
             {
                 if (dest.RuleDefinition != null)
                 {
                     dest.RuleDefinition.RuleEntityId = dest.Id;
                 }

                 if (dest.ActionDefinition != null)
                 {
                     dest.ActionDefinition.RuleEntityId = dest.Id;
                 }

                 if (dest.RuleDefinition?.Parameters != null)
                 {
                     foreach (var param in dest.RuleDefinition.Parameters)
                     {
                         param.RuleDefinitionEntityId = dest.RuleDefinition.Id;
                     }
                 }

                 if (dest.ActionDefinition?.Parameters != null)
                 {
                     foreach (var param in dest.ActionDefinition.Parameters)
                     {
                         param.ActionDefinitionEntityId = dest.ActionDefinition.Id;
                     }
                 }
             });

            CreateMap<RuleDefinitionEntity, IRuleDefinitionVO>()
                .ConvertUsing(dto => MapRuleDefinition(dto));

            CreateMap<IRuleDefinitionVO, RuleDefinitionEntity>()
                .ForMember(dest => dest.RuleEntityId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.RuleDefinitionType))
                .ForMember(dest => dest.Parameters, opt =>
                {
                    opt.UseDestinationValue(); // Keep EF-tracked collection
                    opt.MapFrom(src => src.Parameters);
                });

            CreateMap<KeyValuePair<string, string>, RuleParameterEntity>()
            .ForCtorParam("name", opt => opt.MapFrom(src => src.Key))
            .ForCtorParam("value", opt => opt.MapFrom(src => src.Value.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Value.GetType().ToString())) 
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RuleDefinitionEntityId, opt => opt.Ignore())
            .EqualityComparison((src, dest) => src.Key == dest.Name && src.Value == dest.Value);

            CreateMap<ActionDefinitionEntity, ActionDefinitionVO>()
               .ConvertUsing(dto => MapActionDefinition(dto));

            CreateMap<ActionDefinitionVO, ActionDefinitionEntity>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Parameters, opt =>
                {
                    opt.UseDestinationValue(); // Keep EF-tracked collection
                    opt.MapFrom(src => src.Parameters); 
                })
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.RuleEntityId, opt => opt.Ignore());

            CreateMap<KeyValuePair<string, string>, ActionParameterEntity>()
            .ForCtorParam("name", opt => opt.MapFrom(src => src.Key))
            .ForCtorParam("value", opt => opt.MapFrom(src => src.Value.ToString()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Value.GetType().ToString())) 
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ActionDefinitionEntityId, opt => opt.Ignore())
            .ForMember(dest => dest.ActionEntity, opt => opt.Ignore())
            .EqualityComparison((src, dest) => src.Key == dest.Name);

            #endregion

            #region RuleEntity(EF)_>Domain.RuleEntity
            CreateMap<Entities.RuleEntity, Thermostat.Domain.Domain.HouseAgg.Rule.RuleEntity>()
               .ConstructUsing((src, ctx) =>
               {
                   var sensor = ctx.Items["Sensor"] as Entities.SensorEntity
                       ?? throw new ArgumentNullException("Sensor not provided for mapping RuleEntity");

                   var ruleDefinition = ctx.Mapper.Map<IRuleDefinitionVO>(src.RuleDefinition);
                   var action = ctx.Mapper.Map<ActionDefinitionVO>(src.ActionDefinition);

                   return new Thermostat.Domain.Domain.HouseAgg.Rule.RuleEntity(
                       id: src.Id,
                       sensor: MapSensor(sensor),
                       name: new RuleNameVO(src.Name),
                       ruleDefinition: ruleDefinition,
                       action: action,
                       enabled: src.Enabled
                   );
               });

            CreateMap<RuleDefinitionEntity, IRuleDefinitionVO>()
                .ConvertUsing(src =>
                    new RuleDefinitionAbstractFactory().Create(
                        src.Type,
                        src.Parameters.ToDictionary(p => p.Name, p => p.Value)
                    )
                );

            CreateMap<ActionDefinitionEntity, ActionDefinitionVO>()
                .ConvertUsing(MapHeqatingControllAction);


            #endregion
        }

        private ActionDefinitionVO MapHeqatingControllAction(ActionDefinitionEntity src, ActionDefinitionVO vO)
        {
            if (src.Type == "HeatingControl")
            {
                if (src.Parameters == null || !src.Parameters.Any())
                    throw new InvalidOperationException($"HeatingControl action requires at least one parameter. ActionDefinition Id: {src.Id}");

                return new HeatingControlActionFactory().CreateCustomAction(src.Parameters.First().Value);
            }

            throw new NotSupportedException($"Action type '{src.Type}' is not supported.");
        }


        private Domain.Domain.HouseAgg.Rule.RuleEntity CreateRuleDefinition(Entities.RuleEntity r, HouseEntity house)
        {
            return new Thermostat.Domain.Domain.HouseAgg.Rule.RuleEntity(
                        r.Id,
                        MapSensor(house.Sensors.Single(s => s.Id == r.SensorId)),
                        new RuleNameVO(r.Name),
                        MapRuleDefinition(r.RuleDefinition),
                        MapActionDefinition(r.ActionDefinition),
                        r.Enabled);

        }

        private Domain.Domain.HouseAgg.Sensor.SensorEntity MapSensor(Entities.SensorEntity sensorDto)
        {
            return new Thermostat.Domain.Domain.HouseAgg.Sensor.SensorEntity(
                sensorDto.Id,
                new SensorTypeVO(sensorDto.Type),
                new SensorUnitVO(sensorDto.Unit),
                MapRoom(sensorDto.RoomName, sensorDto.RoomArea, sensorDto.RoomType),
                sensorDto.Measurements.Select(x => new SensorMeasurementVO(x.Timestamp, x.Value)).ToList());
        }
        private static IRuleDefinitionVO MapRuleDefinition(RuleDefinitionEntity dto)
        {
            return new RuleDefinitionAbstractFactory().Create(dto.Type, dto.Parameters.ToDictionary(k => k.Name, el => el.Value));
        }

        private static ActionDefinitionVO MapActionDefinition(ActionDefinitionEntity dto)
        {
            return new ActionDefinitionAbstractFactory().Create(dto.Type, dto.Parameters.ToDictionary(k => k.Name, el => el.Value));
        }
        private RoomVO MapRoom(string roomName, double roomArea, string roomType)
        {
            return new RoomVO(
                roomName,
                roomArea,
                Enum.Parse<RoomType>(roomType)
            );
        }
    }
}
