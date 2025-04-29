using System.Collections.Generic;
using Newtonsoft.Json;

public static class AutoDirtyTracker
{
    private static readonly Dictionary<object, string> _originalSnapshots = new();

    public static void Register(object obj)
    {
        if (obj == null) return;

        string snapshot = JsonConvert.SerializeObject(obj);
        _originalSnapshots[obj] = snapshot;
    }

    public static bool IsDirty(object obj)
    {
        if (obj == null) return false;
        if (!_originalSnapshots.TryGetValue(obj, out var original)) return true;

        string current = JsonConvert.SerializeObject(obj);
        return !current.Equals(original);
    }

    public static void UpdateSnapshot(object obj)
    {
        if (obj == null) return;

        string snapshot = JsonConvert.SerializeObject(obj);
        _originalSnapshots[obj] = snapshot;
    }

    public static void Unregister(object obj)
    {
        if (obj == null) return;

        _originalSnapshots.Remove(obj);
    }
}
