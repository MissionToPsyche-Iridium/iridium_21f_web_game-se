using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Config
{
    private const string PATH = "Config";

    private static RootConfig _config = JsonUtilityWrapper.FromJson<RootConfig>(Resources.Load<TextAsset>(PATH).text);

    public static T Get<T>(string propertyPath)
    {
        string[] properties = propertyPath.Split('.');
        object obj = _config;
        foreach (string property in properties)
        {
            obj = obj.GetType().GetField(property).GetValue(obj);
        }
        return (T) obj;
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
            public int Hp, Armor, FuelCapacity, Speed, ScanningRange;
        }
    }
}
