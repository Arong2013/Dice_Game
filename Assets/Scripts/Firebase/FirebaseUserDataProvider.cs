using Firebase.Database;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class FirebaseUserDataProvider<T> : IUserDataProvider<T>
{
    private readonly string _node;
    private readonly DatabaseReference _rootRef;
    public FirebaseUserDataProvider(string node)
    {
        _node = node;
        _rootRef = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public async Task<T> LoadAsync(string uid)
    {
        var snapshot = await _rootRef.Child(_node).Child(uid).GetValueAsync();

        if (!snapshot.Exists)
            return default;

        string json = snapshot.GetRawJsonValue();
        return JsonConvert.DeserializeObject<T>(json);
    }
    public async Task SaveAsync(string uid, T data)
    {
        string json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Include
        });

        await _rootRef.Child(_node).Child(uid).SetRawJsonValueAsync(json);
    }
}
