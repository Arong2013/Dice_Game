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

    [MenuItem("Tools/Data/TSV → ScriptableObject 자동 등록")]
    public static void RegisterAllTSVs()
    {
        if (!Directory.Exists(tsvFolder))
        {
            Debug.LogError($"[TSVLoader] TSV 폴더가 없습니다: {tsvFolder}");
            return;
        }

        string[] tsvFiles = Directory.GetFiles(tsvFolder, "*.tsv", SearchOption.TopDirectoryOnly);

        foreach (string filePath in tsvFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);   // e.g. "CharacterData"
            string className = fileName + "SO";                             // e.g. "CharacterDataSO"

            // 타입 찾기 (ScriptableObject 기반)
            Type soType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t =>
                    t.Name.Equals(className, StringComparison.OrdinalIgnoreCase) &&
                    typeof(ScriptableObject).IsAssignableFrom(t) &&
                    !t.IsAbstract
                );

            if (soType == null)
            {
                Debug.LogWarning($"[TSVLoader] {className} 타입이 없습니다. 스킵합니다.");
                continue;
            }

            // 제네릭 메서드 호출
            MethodInfo method = typeof(TSVLoaderEditor)
                .GetMethod(nameof(GenerateSOFromTSV), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(soType);

            string tsv = File.ReadAllText(filePath);
            method.Invoke(null, new object[] { tsv });
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[TSVLoader] TSV → SO 변환 완료!");
    }

    // 제네릭으로 SO 생성
    private static void GenerateSOFromTSV<T>(string tsv) where T : ScriptableObject
    {
        TSVScriptableObjectGenerator.ParseToScriptableObjects<T>(tsv, outputFolder);
    }
}
