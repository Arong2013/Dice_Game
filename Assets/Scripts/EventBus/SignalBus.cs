using System;
using System.Collections.Generic;
using System.Linq;
[AttributeUsage(AttributeTargets.Class)]
public class AutoSignalHandlersAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class SignalHandlerAttribute : Attribute { }
public class SignalBus : ISignalBus
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public void Subscribe<T>(Action<T> handler)
    {
        var type = typeof(T);
        if (!_subscribers.ContainsKey(type))
            _subscribers[type] = new List<Delegate>();

        _subscribers[type].Add(handler);
    }

    public void Unsubscribe<T>(Action<T> handler)
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var list))
            list.Remove(handler);
    }

    public void Fire<T>(T signal)
    {
        if (_subscribers.TryGetValue(typeof(T), out var list))
        {
            foreach (var del in list.Cast<Action<T>>().ToList())
                del(signal);
        }
    }
}
