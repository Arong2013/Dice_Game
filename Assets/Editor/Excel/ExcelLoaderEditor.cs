using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Reflection;
using ExcelDataReader;
using System.Collections.Generic;
using System.Data;

public static class ExcelLoaderEditor
{
    private const string excelFolder = "Assets/Excel";
    private const string outputFolder = "Assets/Resources/SO";

    [MenuItem("Tools/Data/Excel(All) �� ScriptableObject �ڵ� ���")]
    public static void RegisterAllFromExcels()
    {
        if (!Directory.Exists(excelFolder))
        {
            Debug.LogError($"[ExcelLoader] Excel ������ �����ϴ�: {excelFolder}");
            return;
        }

        var excelFiles = Directory.GetFiles(excelFolder, "*.xlsx", SearchOption.TopDirectoryOnly);
        if (excelFiles.Length == 0)
        {
            Debug.LogWarning("[ExcelLoader] ó���� ���� ������ �����ϴ�.");
            return;
        }

        foreach (var excelPath in excelFiles)
        {
            using var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            var dataset = reader.AsDataSet();

            foreach (DataTable table in dataset.Tables)
            {
                string sheetName = table.TableName.Trim();
                string className = sheetName + "SO";

                // Ÿ�� ã��
                Type soType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t =>
                        t.Name.Equals(className, StringComparison.OrdinalIgnoreCase) &&
                        typeof(ScriptableObject).IsAssignableFrom(t) &&
                        !t.IsAbstract
                    );

                if (soType == null)
                {
                    Debug.LogWarning($"[ExcelLoader] ��Ʈ '{sheetName}' ���� Ÿ�� {className} ����. ��ŵ.");
                    continue;
                }

                MethodInfo method = typeof(ExcelScriptableObjectGenerator)
                    .GetMethod("GenerateFromExcel", BindingFlags.Public | BindingFlags.Static)
                    .MakeGenericMethod(soType);

                method.Invoke(null, new object[] { table, outputFolder });
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[ExcelLoader] Excel �� SO ��ü ��ȯ �Ϸ�!");
    }
}
