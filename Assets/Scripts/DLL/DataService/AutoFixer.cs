using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public static class AutoFixer
{
    public static void Fix<T>(T target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        FixObject(target);
    }

    private static void FixObject(object obj)
    {
        if (obj == null) return;

        var type = obj.GetType();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var value = field.GetValue(obj);
            var fieldType = field.FieldType;

            if (value == null)
            {
                if (TryConvertPreviousValue(field, obj))
                    continue;

                if (fieldType.IsClass && fieldType != typeof(string))
                {
                    object instance = null;

                    if (typeof(IList).IsAssignableFrom(fieldType))
                        instance = CreateList(fieldType);
                    else if (typeof(IDictionary).IsAssignableFrom(fieldType))
                        instance = CreateDictionary(fieldType);
                    else if (fieldType.GetConstructor(Type.EmptyTypes) != null)
                        instance = Activator.CreateInstance(fieldType);

                    if (instance != null)
                        field.SetValue(obj, instance);
                }
                else if (fieldType.IsValueType)
                {
                    var instance = Activator.CreateInstance(fieldType);
                    field.SetValue(obj, instance);
                }
            }

            var fixedValue = field.GetValue(obj);

            if (fixedValue != null)
            {
                if (fieldType.IsClass && fieldType != typeof(string))
                {
                    if (typeof(IList).IsAssignableFrom(fieldType))
                        FixList((IList)fixedValue);
                    else if (typeof(IDictionary).IsAssignableFrom(fieldType))
                        FixDictionary((IDictionary)fixedValue);
                    else
                        FixObject(fixedValue);
                }
            }
        }
    }

    private static bool TryConvertPreviousValue(FieldInfo field, object obj)
    {
        var type = obj.GetType();
        var backupField = type.GetField($"<{field.Name}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
        if (backupField == null) return false;

        var oldValue = backupField.GetValue(obj);
        if (oldValue == null) return false;

        try
        {
            object converted = ConvertTo(oldValue, field.FieldType);
            field.SetValue(obj, converted);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static object ConvertTo(object value, Type type)
    {
        if (value == null)
            return null;

        var valueStr = value.ToString();

        if (Nullable.GetUnderlyingType(type) is Type inner)
            return string.IsNullOrEmpty(valueStr) ? null : ConvertTo(valueStr, inner);

        if (type.IsEnum)
            return Enum.Parse(type, valueStr, ignoreCase: true);

        return Convert.ChangeType(valueStr, type);
    }

    private static void FixList(IList list)
    {
        if (list == null) return;

        foreach (var item in list)
        {
            if (item != null)
                FixObject(item);
        }
    }

    private static void FixDictionary(IDictionary dict)
    {
        if (dict == null) return;

        foreach (var value in dict.Values)
        {
            if (value != null)
                FixObject(value);
        }
    }

    private static object CreateList(Type listType)
    {
        if (listType.IsInterface && listType.IsGenericType)
        {
            var genericType = typeof(List<>).MakeGenericType(listType.GetGenericArguments()[0]);
            return Activator.CreateInstance(genericType);
        }
        else if (!listType.IsAbstract)
        {
            return Activator.CreateInstance(listType);
        }
        return null;
    }

    private static object CreateDictionary(Type dictType)
    {
        if (dictType.IsInterface && dictType.IsGenericType)
        {
            var args = dictType.GetGenericArguments();
            var genericType = typeof(Dictionary<,>).MakeGenericType(args[0], args[1]);
            return Activator.CreateInstance(genericType);
        }
        else if (!dictType.IsAbstract)
        {
            return Activator.CreateInstance(dictType);
        }
        return null;
    }
}
