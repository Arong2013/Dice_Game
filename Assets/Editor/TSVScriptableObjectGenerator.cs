#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
public static class TSVScriptableObjectGenerator
{
#if UNITY_EDITOR
    public static void ParseToScriptableObjects<T>(string tsv, string outputFolder) where T : ScriptableObject
    {
        var lines = tsv.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2)
        {
            Debug.LogWarning("[TSVParser] TSV 데이터가 없습니다.");
            return;
        }

        var headers = lines[0].Split('\t').Select(h => h.Trim()).ToArray();
        var fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        // 기본 저장 경로: Assets/SO/타입이름
        string typeFolder = Path.Combine(outputFolder, typeof(T).Name);
        if (!Directory.Exists(typeFolder))
            Directory.CreateDirectory(typeFolder);

        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split('\t');
            var so = ScriptableObject.CreateInstance<T>();

            string nameValue = null;

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
                    field.SetValue(so, parsed);

                    if (field.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                        nameValue = parsed.ToString();
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[TSVParser] {headers[j]} 파싱 실패 (줄 {i + 1}): {e.Message}");
                }
            }

            if (string.IsNullOrEmpty(nameValue))
            {
                Debug.LogError($"[TSVParser] name 필드가 비어있습니다. 줄 {i + 1}");
                continue;
            }

            string assetPath = Path.Combine(typeFolder, $"{nameValue}.asset").Replace("\\", "/");
            AssetDatabase.CreateAsset(so, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[TSVParser] {typeof(T).Name} ScriptableObject {lines.Length - 1}개 생성 완료!");
    }
    private static object ConvertTo(string value, Type type)
    {
        if (Nullable.GetUnderlyingType(type) is Type inner)
            return string.IsNullOrEmpty(value) ? null : ConvertTo(value, inner);
        if (type.IsEnum)
            return Enum.Parse(type, value);
        return Convert.ChangeType(value, type);
    }

    private static bool IsList(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);

    private static object ParseList(string raw, Type listType)
    {
        var items = raw.Trim('[', ']').Split(',');
        var elemType = listType.GetGenericArguments()[0];
        var list = (IList)Activator.CreateInstance(listType);
        foreach (var item in items)
            list.Add(ConvertTo(item.Trim(), elemType));
        return list;
    }
#endif
}
