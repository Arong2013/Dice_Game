using System;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public class InjectAttribute : Attribute
{
    public string Id { get; }

    public InjectAttribute() { }
    public InjectAttribute(string id)
    {
        Id = id;
    }
}
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class InjectOptionalAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method)]
public class PostInjectAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class)]
public class AutoRegisterInContainerAttribute : Attribute { }