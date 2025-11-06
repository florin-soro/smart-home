namespace Thermostat.Domain.Domain.HouseAgg.Rule
{
    public abstract record ActionDefinitionVO(string Type)
    {
        public abstract IReadOnlyDictionary<string, string> Parameters { get; }
    }
}
