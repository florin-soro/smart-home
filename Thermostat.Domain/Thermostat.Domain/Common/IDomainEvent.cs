namespace Thermostat.Domain.Common
{
    public interface IDomainEvent
    {
        public DateTime OccurredOn { get; }
    }
}
