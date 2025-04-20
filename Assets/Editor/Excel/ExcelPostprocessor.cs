using UnityEditor;
using UnityEngine;
using System.IO;
using System.Data;
using ExcelDataReader;
using System.Reflection;
using System.Linq;
using System;

public class ExcelPostprocessor : AssetPostprocessor
{
    private const string excelFolder = "Assets/Excel";
    private const string outputFolder = "Assets/Resources/SO";

    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (var path in importedAssets)
        {
            if (!path.EndsWith(".xlsx") || !path.StartsWith(excelFolder))
                continue;

            Debug.Log($"[ExcelPostprocessor] 감지된 엑셀 파일: {path}");

            using var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var dataset = reader.AsDataSet();

            foreach (DataTable table in dataset.Tables)
            {
                string sheetName = table.TableName.Trim();
                string className = sheetName + "SO";

                var soType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t =>
                        t.Name.Equals(className, StringComparison.OrdinalIgnoreCase) &&
                        typeof(ScriptableObject).IsAssignableFrom(t) &&
                        !t.IsAbstract);

                if (soType == null)
                {
                    Debug.LogWarning($"[ExcelPostprocessor] 시트 '{sheetName}' 대응 타입 {className} 없음. 스킵.");
                    continue;
                }

                var method = typeof(ExcelScriptableObjectGenerator)
                    .GetMethod("GenerateFromExcel", BindingFlags.Public | BindingFlags.Static)
                    ?.MakeGenericMethod(soType);

                method?.Invoke(null, new object[] { table, outputFolder });
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
