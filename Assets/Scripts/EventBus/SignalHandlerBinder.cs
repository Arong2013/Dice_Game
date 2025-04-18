using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class SignalHandlerBinder
{
    private static readonly Dictionary<object, List<(Type, Delegate)>> _subscriptions = new();

    public static void AutoRegisterHandlers(object instance, ISignalBus bus)
    {
        if (!_subscriptions.ContainsKey(instance))
            _subscriptions[instance] = new();

        var methods = instance.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.GetCustomAttribute<SignalHandlerAttribute>() != null);

        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            if (parameters.Length != 1) continue;

            var paramType = parameters[0].ParameterType;
            var actionType = typeof(Action<>).MakeGenericType(paramType);
            var del = Delegate.CreateDelegate(actionType, instance, method);

            typeof(ISignalBus)
                .GetMethod(nameof(ISignalBus.Subscribe))
                .MakeGenericMethod(paramType)
                .Invoke(bus, new object[] { del });

            _subscriptions[instance].Add((paramType, del));
        }
    }

    public static void UnregisterHandlers(object instance, ISignalBus bus)
    {
        if (!_subscriptions.TryGetValue(instance, out var handlers)) return;

        foreach (var (paramType, del) in handlers)
        {
            typeof(ISignalBus)
                .GetMethod(nameof(ISignalBus.Unsubscribe))
                .MakeGenericMethod(paramType)
                .Invoke(bus, new object[] { del });
        }

        _subscriptions.Remove(instance);
    }
}
