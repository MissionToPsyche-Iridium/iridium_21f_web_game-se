using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    private const string PATH = "Config";

    private static RootConfig _config = JsonUtilityWrapper.FromJson<RootConfig>(Resources.Load<TextAsset>(PATH).text);

    public static object Get(string query)
    {
        // TODO: implement querying?
        return -1;
    }

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
