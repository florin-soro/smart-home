using Thermostat.Domain.Domain.HouseAgg.Rule.RuleContext;

namespace Thermostat.Domain.Domain.HouseAgg.Rule.RuleDefinition
{
    public interface IRuleDefinition<T>:IRuleDefinitionVO where T : IRuleContext
    {
        async Task<bool> IRuleDefinitionVO.IsSatisfiedByAsync(IRuleContext context)
        {
            if (context is T typedContext)
            {
                return await IsSatisfiedByAsync(typedContext);
            }
            else
            {
                return false;
            }
        }
        Task<bool> IsSatisfiedByAsync(T context);

    }

    public interface IRuleDefinitionVO
    {
        Task<bool> IsSatisfiedByAsync(IRuleContext context);
        RuleDefinitionType RuleDefinitionType { get; }
        IReadOnlyDictionary<string, string> Parameters { get; }
    }
}
