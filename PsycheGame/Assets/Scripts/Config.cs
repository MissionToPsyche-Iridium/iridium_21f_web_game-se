using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Config
{
    private const string PATH = "Config";

    private static RootConfig _config = JsonUtilityWrapper.FromJson<RootConfig>(Resources.Load<TextAsset>(PATH).text);
    private static Regex _arrayRegex = new Regex("(?<property>\\S+)\\[(?<index>\\d+)\\]");

    public static T Get<T>(string propertyPath)
    {
        bool lengthModifier = propertyPath.StartsWith('#');
        if (lengthModifier)
        {
            propertyPath = propertyPath.Substring(1);
        }

        string[] properties = propertyPath.Split('.');
        object obj = _config;

        foreach (string property in properties)
        {
            Match match = _arrayRegex.Match(property);
            obj = obj.GetType().GetField(match.Success ? match.Groups["property"].Value : property).GetValue(obj);
            obj = match.Success ? ((object[]) obj)[Int32.Parse(match.Groups["index"].Value)] : obj;
        }

        return (T) (lengthModifier ? ((object[]) obj).Length : obj);
    }

    /**
    public static void Set(string propertyPath, object value)
    {
        string[] properties = propertyPath.Split('.');
        object obj = _config;
        foreach (string property in properties.Take(properties.Length - 1))
        {
            obj = obj.GetType().GetField(property).GetValue(obj);
        }
        obj.GetType().GetField(properties[properties.Length - 1]).SetValue(obj, value);
    }
    */

    [Serializable]
    private class RootConfig
    {
        public ProbeComponent[] ProbeComponents;
        public InventoryConfigEntry[] StartingInventory;
        public MaxAttributesConfig MaxAttributes;

        [Serializable]
        public class InventoryConfigEntry
        {
            public string ProbeComponentId;
            public int Quantity;
        }

        [Serializable]
        public class MaxAttributesConfig
        {
            public int Health, FuelCapacity, Speed, ScanningRange;
        }
    }
}
