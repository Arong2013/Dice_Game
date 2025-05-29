using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using ExcelDataReader;

public static class ExcelWebLoaderEditor
{
    private const string outputFolder = "Assets/Resources/SO";

    [MenuItem("Tools/Data/Excel(웹) → ScriptableObject 자동 등록")]
    public static void RegisterFromWebExcels()
    {
        var source = AssetDatabase.LoadAssetAtPath<ExcelWebSource>("Assets/ExcelWebSource/ExcelWebSource.asset");
        if (source == null)
        {
            Debug.LogError("ExcelWebSource.asset 을 찾을 수 없습니다.");
            return;
        }

        foreach (var entry in source.entries)
        {
            if (string.IsNullOrWhiteSpace(entry.url)) continue;
            if (entry.targetSO == null) continue;

            ProcessExcelForSO(entry.url, entry.targetSO);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[ExcelWebLoader] ✅ 전체 웹 Excel 변환 완료!");
    }

    private static void ProcessExcelForSO(string url, ScriptableObject targetSO)
    {
        try
        {
            byte[] excelBytes = DownloadExcel(url);
            if (excelBytes == null)
            {
                Debug.LogWarning($"[ExcelWebLoader] 다운로드 실패: {url}");
                return;
            }

            using var stream = new MemoryStream(excelBytes);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var dataset = reader.AsDataSet();

            foreach (DataTable table in dataset.Tables)
            {
                string sheetName = table.TableName.Trim();
                AddDataToSO(targetSO, table);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[ExcelWebLoader] 처리 실패: {e.Message}");
        }
    }

    private static void AddDataToSO(ScriptableObject targetSO, DataTable table)
    {
        var soType = targetSO.GetType();
        var fields = soType.GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Where(f => f.FieldType.IsGenericType && 
                        f.FieldType.GetGenericTypeDefinition() == typeof(List<>));

        foreach (var field in fields)
        {
            // 시트 이름과 정확히 일치하는 필드명 찾기
            if (field.Name == table.TableName)
            {
                var listType = field.FieldType.GetGenericArguments()[0];
                var list = field.GetValue(targetSO) as IList;
                PopulateListFromTable(list, listType, table);
                break;
            }
        }

        EditorUtility.SetDirty(targetSO);
    }

    private static void PopulateListFromTable(IList list, Type listType, DataTable table)
    {
        if (list == null || table.Rows.Count < 2) return;

        // 첫 번째 행을 헤더로 사용
        var headers = table.Rows[0].ItemArray.Select(h => h.ToString().Trim()).ToArray();
        var fields = listType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        // 데이터 행부터 시작 (1번 인덱스부터)
        for (int i = 1; i < table.Rows.Count; i++)
        {
            var values = table.Rows[i].ItemArray;
            var instance = Activator.CreateInstance(listType);

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                var field = fields.FirstOrDefault(f => 
                    f.Name.Equals(headers[j], StringComparison.OrdinalIgnoreCase));
                
                if (field == null) continue;

                try
                {
                    var raw = values[j]?.ToString().Trim();
                    object parsed = IsList(field.FieldType)
                        ? ParseList(raw, field.FieldType)
                        : ConvertTo(raw, field.FieldType);
                    
                    field.SetValue(instance, parsed);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[ExcelParser] 필드 '{headers[j]}' 파싱 실패 (줄 {i + 1}): {e.Message}");
                }
            }

            list.Add(instance);
        }
    }

    private static byte[] DownloadExcel(string url)
    {
        try
        {
            using var client = new System.Net.WebClient();
            return client.DownloadData(url);
        }
        catch (Exception e)
        {
            Debug.LogError($"[ExcelWebLoader] 다운로드 에러: {e.Message}");
            return null;
        }
    }

    private static object ConvertTo(string value, Type type)
    {
        if (string.IsNullOrEmpty(value)) return null;

        if (Nullable.GetUnderlyingType(type) is Type inner)
            return string.IsNullOrEmpty(value) ? null : ConvertTo(value, inner);

        if (type.IsEnum)
            return Enum.Parse(type, value, true);

        // 특별한 변환 필요한 타입들 처리
        if (type == typeof(int)) return int.Parse(value);
        if (type == typeof(float)) return float.Parse(value);
        if (type == typeof(bool)) return bool.Parse(value);
        if (type == typeof(DateTime)) return DateTime.Parse(value);

        return Convert.ChangeType(value, type);
    }

    private static bool IsList(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);

    private static object ParseList(string raw, Type listType)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        var elemType = listType.GetGenericArguments()[0];
        var list = (IList)Activator.CreateInstance(listType);

        // CSV 형식의 리스트 파싱
        var items = raw.Trim('[', ']').Split(',');
        foreach (var item in items)
        {
            list.Add(ConvertTo(item.Trim(), elemType));
        }

        return list;
    }
}