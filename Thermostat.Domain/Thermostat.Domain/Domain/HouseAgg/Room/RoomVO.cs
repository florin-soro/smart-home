using static Thermostat.Domain.Shared.Guard;

namespace Thermostat.Domain.Domain.HouseAgg.Room
{
    public record RoomVO
    {
        public RoomVO(string name, double area, RoomType roomType)
        {
            Name=  StringNotNullOrEmpty(name, nameof(name));
            Area = Check(area, area => area > 0, $"{nameof(area)} must be greater than 0.");
            RoomType = ValidEnum(roomType, nameof(roomType));
        }
        public string Name { get; } 

        public double Area { get; }

        public RoomType RoomType { get; } 
    }
}
