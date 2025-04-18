using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class MyContainer
{
    private List<BindingInfo> _bindings = new();
    public Binder<T> Bind<T>()
    {
        return new Binder<T>(this);
    }
    internal void Register(Type interfaceType, Type implementationType, Lifetime lifetime, string id = null)
    {
        _bindings.Add(new BindingInfo
        {
            InterfaceType = interfaceType,
            ImplementationType = implementationType,
            Lifetime = lifetime,
            Id = id
        });
    }
    public T Resolve<T>(string id = null) => (T)Resolve(typeof(T), id);
    public object Resolve(Type type, string id = null)
    {
        var binding = _bindings.FirstOrDefault(b => b.InterfaceType == type && b.Id == id);

        if (binding == null)
            throw new Exception($"No binding found for {type.Name} (id: {id ?? "none"})");

        if (binding.Lifetime == Lifetime.Singleton && binding.SingletonInstance != null)
            return binding.SingletonInstance;

        var implType = binding.ImplementationType;
        var constructors = implType.GetConstructors();
        var injectCtor = constructors.FirstOrDefault(c => c.GetCustomAttribute<InjectAttribute>() != null)
                      ?? constructors.OrderBy(c => c.GetParameters().Length).FirstOrDefault();

        var parameters = injectCtor.GetParameters();
        var args = parameters.Select(p => 
        {
            var attr = p.GetCustomAttribute<InjectAttribute>();
            return Resolve(p.ParameterType, attr?.Id);
        }).ToArray();

        var instance = injectCtor.Invoke(args);

        foreach (var field in implType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            var injectAttr = field.GetCustomAttribute<InjectAttribute>();
            if (injectAttr != null)
                field.SetValue(instance, Resolve(field.FieldType, injectAttr.Id));
            else if (field.GetCustomAttribute<InjectOptionalAttribute>() != null)
                field.SetValue(instance, TryResolve(field.FieldType));
        }

        foreach (var prop in implType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            var injectAttr = prop.GetCustomAttribute<InjectAttribute>();
            if (injectAttr != null)
                prop.SetValue(instance, Resolve(prop.PropertyType, injectAttr.Id));
            else if (prop.GetCustomAttribute<InjectOptionalAttribute>() != null)
                prop.SetValue(instance, TryResolve(prop.PropertyType));
        }

        foreach (var method in implType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            if (method.GetCustomAttribute<PostInjectAttribute>() != null)
                method.Invoke(instance, null);
        }


        if (binding.Lifetime == Lifetime.Singleton)
            binding.SingletonInstance = instance;

        return instance;
    }

    public object TryResolve(Type type)
    {
        try
        {
            return Resolve(type);
        }
        catch
        {
            return null;
        }
    }

    public void RegisterInstance(Type type, object instance, string id = null)
    {
        _bindings.Add(new BindingInfo
        {
            InterfaceType = type,
            ImplementationType = instance.GetType(),
            Lifetime = Lifetime.Singleton,
            SingletonInstance = instance,
            Id = id
        });
    }
}
