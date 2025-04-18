using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Reflection;

public static class TSVLoaderEditor
{
    private const string tsvFolder = "Assets/TSV";
    private const string outputFolder = "Assets/Resources/SO";

    [MenuItem("Tools/Data/TSV �� ScriptableObject �ڵ� ���")]
    public static void RegisterAllTSVs()
    {
        if (!Directory.Exists(tsvFolder))
        {
            Debug.LogError($"[TSVLoader] TSV ������ �����ϴ�: {tsvFolder}");
            return;
        }

        string[] tsvFiles = Directory.GetFiles(tsvFolder, "*.tsv", SearchOption.TopDirectoryOnly);

        foreach (string filePath in tsvFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);   // e.g. "CharacterData"
            string className = fileName + "SO";                             // e.g. "CharacterDataSO"

            // Ÿ�� ã�� (ScriptableObject ���)
            Type soType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    t.Name.Equals(className, StringComparison.OrdinalIgnoreCase) &&
                    typeof(ScriptableObject).IsAssignableFrom(t) &&
                    !t.IsAbstract
                );

            if (soType == null)
            {
                Debug.LogWarning($"[TSVLoader] {className} Ÿ���� �����ϴ�. ��ŵ�մϴ�.");
                continue;
            }

            // ���׸� �޼��� ȣ��
            MethodInfo method = typeof(TSVLoaderEditor)
                .GetMethod(nameof(GenerateSOFromTSV), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(soType);

            string tsv = File.ReadAllText(filePath);
            method.Invoke(null, new object[] { tsv });
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[TSVLoader] TSV �� SO ��ȯ �Ϸ�!");
    }

    // ���׸����� SO ����
    private static void GenerateSOFromTSV<T>(string tsv) where T : ScriptableObject
    {
        TSVScriptableObjectGenerator.ParseToScriptableObjects<T>(tsv, outputFolder);
    }
}
