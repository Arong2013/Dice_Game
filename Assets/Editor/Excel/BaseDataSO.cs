using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public abstract class BaseDataSO : ScriptableObject
{
    // 모든 데이터 리스트 가져오기
    public List<object> GetAllDataLists()
    {
        return GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Where(f => f.FieldType.IsGenericType && 
                        f.FieldType.GetGenericTypeDefinition() == typeof(List<>))
            .Select(f => f.GetValue(this))
            .ToList();
    }

    // 특정 타입의 리스트 찾기
    public List<T> GetDataList<T>()
    {
        return GetType()
            .GetFields(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(f => f.FieldType == typeof(List<T>))
            ?.GetValue(this) as List<T>;
    }
}