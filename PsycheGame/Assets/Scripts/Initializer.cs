using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField]
    private TextAsset probeComponentConfig, probeComponentInventoryConfig;

    public void Awake()
    {
        List<Tuple<ProbeComponent, int>> startingInventory = new List<Tuple<ProbeComponent, int>>();
        InventoryConfigEntry[] entries = JsonParser.FromJsonArray<InventoryConfigEntry>(probeComponentInventoryConfig.text);
        foreach (ProbeComponent probeComponent in JsonParser.FromJsonArray<ProbeComponent>(probeComponentConfig.text))
        {
            foreach (InventoryConfigEntry entry in entries)
            {
                if (entry.ProbeComponentId.Equals(probeComponent.Id))
                {
                    startingInventory.Add(new Tuple<ProbeComponent, int>(probeComponent, entry.Quantity));
                }
            }
        }

        Player player = Player.GetInstance();
        player.Initialize(startingInventory);

        BuildManager.GetInstance().Initialize();
    }

    [Serializable]
    private class InventoryConfigEntry
    {
        public string ProbeComponentId;
        public int Quantity;
    }
}
