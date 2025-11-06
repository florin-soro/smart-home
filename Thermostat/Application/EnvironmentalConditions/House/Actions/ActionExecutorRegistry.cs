using Thermostat.Domain.Domain.HouseAgg.Rule;

namespace Thermostat.Application.EnvironmentalConditions.House.Actions
{
    public class ActionExecutorRegistry
    {
        private readonly Dictionary<Type, IActionExecutor> _executors;

        public ActionExecutorRegistry(Dictionary<Type, IActionExecutor> executors)
        {
            _executors = executors;
        }

        public IActionExecutor Resolve(ActionDefinitionVO action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action), "Action cannot be null");
            if (_executors.TryGetValue(action.GetType(), out var executor))
                return executor;
            throw new KeyNotFoundException($"No executor found for action type: {action.GetType().Name}");
        }


        public IActionExecutor Resolve(Type actionType)
        {
            if (_executors.TryGetValue(actionType, out var executor))
                return executor;

            throw new KeyNotFoundException($"No executor found for action type: {actionType.GetType().Name}");
        }

        public IActionExecutor Resolve<T>() where T : ActionDefinitionVO => Resolve(typeof(T));
    }

    public class ActionExecutorRegistryBuilder
    {
        private readonly Dictionary<Type, IActionExecutor> _executors = new();

        public ActionExecutorRegistryBuilder Register<TAction>(IActionExecutor executor) where TAction : ActionDefinitionVO
        {
            _executors[typeof(TAction)] = executor;
            return this;
        }

        public ActionExecutorRegistry Build()
        {
            return new ActionExecutorRegistry(_executors);
        }
    }
}
