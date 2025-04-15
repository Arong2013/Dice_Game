using System;

public class Binder<T>
{
    private readonly MyContainer _container;
    private Type _implType;
    private string _id;

    public Binder(MyContainer container)
    {
        _container = container;
    }

    public Binder<T> To<U>() where U : T
    {
        _implType = typeof(U);
        return this;
    }

    public Binder<T> WithId(string id)
    {
        _id = id;
        return this;
    }

    public void AsTransient()
    {
        _container.Register(typeof(T), _implType, Lifetime.Transient, _id);
    }

    public void AsSingleton()
    {
        _container.Register(typeof(T), _implType, Lifetime.Singleton, _id);
    }
}
