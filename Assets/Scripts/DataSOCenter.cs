using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public static class DataSOCenter
{
    private static readonly Dictionary<Type, IDictionary> _typeToDict = new();
    public static void Init()
    {
        // Resources 폴더 안의 모든 ScriptableObject 로드
        var allSOs = Resources.LoadAll<ScriptableObject>("");

        foreach (var so in allSOs)
        {
            Type type = so.GetType();

            // ID 필드 존재 여부 확인
            var idField = type.GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);
            if (idField == null || idField.FieldType != typeof(int))
            {
                continue; // 무시
            }

            int id = (int)idField.GetValue(so);

            if (!_typeToDict.TryGetValue(type, out var dict))
            {
                var genericDictType = typeof(Dictionary<,>).MakeGenericType(typeof(int), type);
                dict = (IDictionary)Activator.CreateInstance(genericDictType);
                _typeToDict[type] = dict;
            }

            var typedDict = (IDictionary)dict;

            if (typedDict.Contains(id))
            {
                Debug.LogWarning($"[DataCenter] 중복 ID 감지: {type.Name}[{id}]");
                continue;
            }

            typedDict[id] = so;
        }

        Debug.Log($"[DataCenter] 초기화 완료: 총 {_typeToDict.Count} 타입 등록됨");
    }

    public static T Get<T>(int id)
    {
        if (_typeToDict.TryGetValue(typeof(T), out var dict))
        {
            var typedDict = (IDictionary<int, T>)dict;
            return typedDict.TryGetValue(id, out var value) ? value : default;
        }

        return default;
    }

    public static IEnumerable<T> GetAll<T>()
    {
        if (_typeToDict.TryGetValue(typeof(T), out var dict))
        {
            return ((IDictionary<int, T>)dict).Values;
        }

        return Array.Empty<T>();
    }
}
