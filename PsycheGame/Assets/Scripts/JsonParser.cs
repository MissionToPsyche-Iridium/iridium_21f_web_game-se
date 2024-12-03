using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonParser
{
    public static string ToJson(object obj, bool format = false)
    {
        return JsonUtility.ToJson(obj, format);
    }

    public static string ToJsonArray<T>(T[] objs, bool format = false)
    {
        return ToJson(new Wrapper<T>(objs), format);
    }

    public static T FromJson<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }

    public static T[] FromJsonArray<T>(string jsonArray)
    {
        return FromJson<Wrapper<T>>(jsonArray).array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
        public Wrapper(T[] array)
        {
            this.array = array;
        }
    }
}
