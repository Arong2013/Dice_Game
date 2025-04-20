#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Data;
using ExcelDataReader;

public static class ExcelScriptableObjectGenerator
{
    public static void GenerateFromExcel<T>(DataTable sheet, string outputFolder) where T : ScriptableObject
    {
        if (sheet.Rows.Count < 2)
        {
            Debug.LogWarning($"[ExcelParser] {sheet.TableName} 시트에 유효한 데이터가 없습니다.");
            return;
        }

        var headers = sheet.Rows[0].ItemArray.Select(h => h.ToString().Trim()).ToArray();
        var fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        string typeFolder = Path.Combine(outputFolder, typeof(T).Name);
        if (!Directory.Exists(typeFolder))
            Directory.CreateDirectory(typeFolder);

        for (int i = 1; i < sheet.Rows.Count; i++)
        {
            var values = sheet.Rows[i].ItemArray;
            var so = ScriptableObject.CreateInstance<T>();

            string nameValue = null;

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                var field = fields.FirstOrDefault(f => f.Name.Equals(headers[j], StringComparison.OrdinalIgnoreCase));
                if (field == null) continue;

                try
                {
                    var raw = values[j]?.ToString().Trim();
                    object parsed = IsList(field.FieldType)
                        ? ParseList(raw, field.FieldType)
                        : ConvertTo(raw, field.FieldType);
                    field.SetValue(so, parsed);

                    if (field.Name.Equals("name", StringComparison.OrdinalIgnoreCase))
                        nameValue = parsed?.ToString();
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[ExcelParser] {headers[j]} 파싱 실패 (줄 {i + 1}): {e.Message}");
                }
            }

            if (string.IsNullOrEmpty(nameValue))
            {
                Debug.LogError($"[ExcelParser] name 필드가 비어있습니다. 줄 {i + 1}");
                continue;
            }

            string assetPath = Path.Combine(typeFolder, $"{nameValue}.asset").Replace("\\", "/");
            AssetDatabase.CreateAsset(so, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[ExcelParser] {typeof(T).Name} ScriptableObject {sheet.Rows.Count - 1}개 생성 완료!");
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
        if (string.IsNullOrWhiteSpace(raw)) return null;
        var items = raw.Trim('[', ']').Split(',');
        var elemType = listType.GetGenericArguments()[0];
        var list = (IList)Activator.CreateInstance(listType);
        foreach (var item in items)
            list.Add(ConvertTo(item.Trim(), elemType));
        return list;
    }
}
#endif
