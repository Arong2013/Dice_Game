using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class UIInstaller
{
    private readonly MyContainer _container;
    public UIInstaller(MyContainer container) => _container = container;
    public void Install()
    {
        var allMonoBehaviours = UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include,FindObjectsSortMode.None);

        foreach (var mono in allMonoBehaviours)
        {
            var type = mono.GetType();
            if (type.GetCustomAttribute<AutoRegisterInContainerAttribute>() != null)
            {
                var interfaces = mono.GetType().GetInterfaces();
                var leafInterfaces = interfaces
                    .Where(i => interfaces.All(other => other == i || !other.GetInterfaces().Contains(i)))
                    .ToList();
                var iface = leafInterfaces.FirstOrDefault();
                if (iface != null)
                {
                    try
                    {
                        _container.RegisterInstance(iface, mono);
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"[UIAutoBinder] 중복 바인딩 생략됨: {iface.Name} - {e.Message}");
                    }
                }
            }
        }
    }
}
