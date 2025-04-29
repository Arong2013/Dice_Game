using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;

public static class FirebaseUserDataIO
{
    private static readonly DatabaseReference _root = FirebaseDatabase.DefaultInstance.RootReference;
    private static readonly Dictionary<Type, object> _cache = new();
    private static List<Type> _types;

    private static void EnsureScanned()
    {
        if (_types != null)
            return;

        _types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<GameDataAttribute>() != null)
            .ToList();
    }

    private static string GetNodeName(Type type)
    {
        var attr = type.GetCustomAttribute<GameDataAttribute>();
        if (attr == null)
            throw new Exception($"[FirebaseUserDataIO] {type.Name}에는 GameDataAttribute가 없습니다.");

        return attr.DataType.ToString(); // ✅ Enum 이름으로 저장
    }

    public static async Task PreloadAllAsync()
    {
        EnsureScanned();
        foreach (var type in _types)
            await LoadAsync(type);
        foreach (var type in _types)
            await SaveAsync(type);
    }

    private static async Task LoadAsync(Type type)
    {
        string uid = FirebaseAuthService.UserId;
        string nodeName = GetNodeName(type);

        var snapshot = await _root.Child(nodeName).Child(uid).GetValueAsync();

        object data = snapshot.Exists
            ? JsonConvert.DeserializeObject(snapshot.GetRawJsonValue(), type)
            : Activator.CreateInstance(type);

        AutoFixer.Fix(data);
        _cache[type] = data;

        if (!snapshot.Exists)
            await SaveAsync(type);

        Logger.Log($"[FirebaseUserDataIO] Loaded {nodeName} for {uid}");
    }

    private static async Task SaveAsync(Type type)
    {
        if (!_cache.TryGetValue(type, out var data) || data == null)
            throw new Exception($"[FirebaseUserDataIO] {type.Name} 데이터가 없습니다.");

        AutoFixer.Fix(data);

        string uid = FirebaseAuthService.UserId;
        string nodeName = GetNodeName(type);

        string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Include
        });

        await _root.Child(nodeName).Child(uid).SetRawJsonValueAsync(json);

        Logger.Log($"[FirebaseUserDataIO] Saved {nodeName} for {uid}");
    }

    public static T Get<T>() where T : class
    {
        _cache.TryGetValue(typeof(T), out var data);
        return data as T;
    }

    public static async Task ForceSaveAllAsync()
    {
        EnsureScanned();
        foreach (var type in _types)
            await SaveAsync(type);
    }
}
