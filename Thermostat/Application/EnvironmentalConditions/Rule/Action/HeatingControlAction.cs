using Thermostat.Domain.Domain.HouseAgg.Rule;

namespace Thermostat.Application.EnvironmentalConditions.Rule.Action
{
    public record HeatingControlAction : ActionDefinitionVO
    {
        public HeatingControlAction(string command) : base("HeatingControl")
        {
            Command = command;
        }

        public override IReadOnlyDictionary<string, string> Parameters => new Dictionary<string, string>
        {
            { "Command", Command },
        };

        public string Command { get; }
    }
}
