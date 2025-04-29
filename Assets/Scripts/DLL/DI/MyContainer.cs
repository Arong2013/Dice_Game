using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class MyContainer
{
    private List<BindingInfo> _bindings = new();

    public Binder<T> Bind<T>() => new Binder<T>(this);
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

        var instance = CreateInstance(binding);
        Inject(instance);

        if (binding.Lifetime == Lifetime.Singleton)
            binding.SingletonInstance = instance;

        return instance;
    }
    private object CreateInstance(BindingInfo binding)
    {
        var implType = binding.ImplementationType;
        var constructors = implType.GetConstructors();
        var injectCtor = constructors.FirstOrDefault(c => c.GetCustomAttribute<InjectAttribute>() != null)
                        ?? constructors.OrderBy(c => c.GetParameters().Length).First();

        var parameters = injectCtor.GetParameters();
        var args = parameters.Select(p => {
            var attr = p.GetCustomAttribute<InjectAttribute>();
            return Resolve(p.ParameterType, attr?.Id);
        }).ToArray();

        return injectCtor.Invoke(args);
    }
    public void Inject(object instance)
    {
        var type = instance.GetType();
        var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        foreach (var field in type.GetFields(flags))
        {
            var injectAttr = field.GetCustomAttribute<InjectAttribute>();
            if (injectAttr != null)
                field.SetValue(instance, Resolve(field.FieldType, injectAttr.Id));
            else if (field.GetCustomAttribute<InjectOptionalAttribute>() != null)
                field.SetValue(instance, TryResolve(field.FieldType));
        }

        foreach (var prop in type.GetProperties(flags))
        {
            var injectAttr = prop.GetCustomAttribute<InjectAttribute>();
            if (injectAttr != null)
                prop.SetValue(instance, Resolve(prop.PropertyType, injectAttr.Id));
            else if (prop.GetCustomAttribute<InjectOptionalAttribute>() != null)
                prop.SetValue(instance, TryResolve(prop.PropertyType));
        }

        foreach (var method in type.GetMethods(flags))
        {
            if (method.GetCustomAttribute<PostInjectAttribute>() != null)
                method.Invoke(instance, null);
        }
    }
    public object TryResolve(Type type)
    {
        try { return Resolve(type); }
        catch { return null; }
    }
    public void Unbind<T>(string id = null)
    {
        _bindings.RemoveAll(b => b.InterfaceType == typeof(T) && b.Id == id);
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
        Inject(instance);
    }
}
