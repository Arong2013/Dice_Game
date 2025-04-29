using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class AutoKeyDictionaryConverter<TKey, TValue> : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(Dictionary<TKey, TValue>).IsAssignableFrom(objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var dictionary = (Dictionary<TKey, TValue>)value;
        var stringKeyDict = new Dictionary<string, TValue>();

        foreach (var kv in dictionary)
        {
            stringKeyDict[kv.Key.ToString()] = kv.Value;
        }

        serializer.Serialize(writer, stringKeyDict);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var stringKeyDict = serializer.Deserialize<Dictionary<string, TValue>>(reader);
        var result = new Dictionary<TKey, TValue>();

        if (stringKeyDict == null)
            return result;

        foreach (var kv in stringKeyDict)
        {
            try
            {
                TKey key = (TKey)ConvertTo(kv.Key, typeof(TKey));
                result[key] = kv.Value;
            }
            catch
            {
                Logger.LogError($"[AutoKeyDictionaryConverter] 키 변환 실패: '{kv.Key}' → {typeof(TKey).Name}");
            }
        }

        return result;
    }

    private static object ConvertTo(string value, Type targetType)
    {
        if (Nullable.GetUnderlyingType(targetType) is Type innerType)
            return string.IsNullOrEmpty(value) ? null : ConvertTo(value, innerType);

        if (targetType.IsEnum)
            return Enum.Parse(targetType, value, ignoreCase: true);

        return Convert.ChangeType(value, targetType);
    }
}
