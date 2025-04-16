using System;
public enum Lifetime
{
    Transient,
    Singleton
}
public class BindingInfo
{
    public Type InterfaceType;
    public Type ImplementationType;
    public object SingletonInstance;
    public Lifetime Lifetime;
    public string Id;
}
