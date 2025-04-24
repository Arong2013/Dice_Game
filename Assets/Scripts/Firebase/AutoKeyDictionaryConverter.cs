using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class AutoKeyDictionaryConverter<TKey, TValue> : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Dictionary<TKey, TValue>);
    }
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var dict = (Dictionary<TKey, TValue>)value;
        var stringDict = new Dictionary<string, TValue>();

        foreach (var kv in dict)
        {
            stringDict[kv.Key.ToString()] = kv.Value;
        }

        serializer.Serialize(writer, stringDict);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var stringDict = serializer.Deserialize<Dictionary<string, TValue>>(reader);
        var result = new Dictionary<TKey, TValue>();

        foreach (var kv in stringDict)
        {
            try
            {
                TKey key = (TKey)ConvertTo(kv.Key, typeof(TKey));
                result[key] = kv.Value;
            }
            catch
            {
                UnityEngine.Debug.LogWarning($"[AutoKeyConverter] 키 변환 실패: '{kv.Key}' → {typeof(TKey).Name}");
            }
        }

        return result;
    }

    private static object ConvertTo(string value, Type type)
    {
        if (Nullable.GetUnderlyingType(type) is Type inner)
            return string.IsNullOrEmpty(value) ? null : ConvertTo(value, inner);

        if (type.IsEnum)
            return Enum.Parse(type, value, ignoreCase: true);

        return Convert.ChangeType(value, type);
    }
}
