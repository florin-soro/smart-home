using static Thermostat.Domain.Shared.Guard;

namespace Thermostat.Domain.Domain.HouseAgg.Rule
{
    public record RuleNameVO
    {
        public string Value { get; }

        public RuleNameVO(string value)
        {//todo extract magic strings
            Value = CheckMultiple(value, $"{nameof(RuleNameVO)}.{nameof(value)}", IsStringNotNullOrEmpty, (n, p) => n.Length > 0 && n.Length < 100);
        }
    }


}
