using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
public static class TSVParser
{
    public static List<T> Parse<T>(string tsv) where T : new()
    {
        var list = new List<T>();
        var lines = tsv.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2) return list;

        var headers = lines[0].Split('\t').Select(h => h.Trim()).ToArray();
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split('\t');
            var instance = new T();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                var field = fields.FirstOrDefault(f => f.Name.Equals(headers[j], StringComparison.OrdinalIgnoreCase));
                if (field == null) continue;

                try
                {
                    var raw = values[j].Trim();
                    object parsed = IsList(field.FieldType)
                        ? ParseList(raw, field.FieldType)
                        : ConvertTo(raw, field.FieldType);
                    field.SetValue(instance, parsed);
                }
                catch { /* 무시 */ }
            }

            list.Add(instance);
        }

        //Debug.Log($"[TSVParser] {typeof(T).Name} 파싱 완료: {list.Count}개");
        return list;
    }
    private static object ConvertTo(string value, Type type)
    {
        if (Nullable.GetUnderlyingType(type) is Type inner)
            return string.IsNullOrEmpty(value) ? null : ConvertTo(value, inner);
        if (type.IsEnum)
            return Enum.Parse(type, value);
        return Convert.ChangeType(value, type);
    }

    private static bool IsList(Type type) =>  type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
    private static object ParseList(string raw, Type listType)
    {
        var items = raw.Trim('[', ']').Split(',');
        var elemType = listType.GetGenericArguments()[0];
        var list = (IList)Activator.CreateInstance(listType);
        foreach (var item in items)
            list.Add(ConvertTo(item.Trim(), elemType));
        return list;
    }
}
