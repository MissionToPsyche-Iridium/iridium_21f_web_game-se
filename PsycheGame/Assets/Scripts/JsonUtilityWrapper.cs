using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class JsonUtilityWrapper
{
    public static string ToJson(object obj, bool format = false)
    {
        return JsonUtility.ToJson(obj, format);
    }

    public static T FromJson<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }

    public static string ToJsonArray<T>(T[] objs, bool format = false)
    {
        return ToJson(new ArrayContainer<T>(objs), format);
    }

    public static T[] FromJsonArray<T>(string jsonArray)
    {
        return FromJson<ArrayContainer<T>>(jsonArray).array;
    }

    public static string DictionaryToJson(Dictionary<string, int> dictionary, bool format = false)
    {
        string json = "{" + (format ? "\n\t" : "");
        for (int i = 0; i < dictionary.Count; i++)
        {
            json += $"\"{dictionary.Keys.ElementAt<string>(i)}\": {dictionary.Values.ElementAt<int>(i).ToString()}";
            if (i != dictionary.Count - 1)
            {
                json += "," + (format ? "\n\t" : " ");
            }
            else if (format)
            {
                json += "\n";
            }
        }
        json += "}";
        return json;
    }

    [Serializable]
    private class ArrayContainer<T>
    {
        public T[] array;
        public ArrayContainer(T[] array)
        {
            this.array = array;
        }
    }
}
