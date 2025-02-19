using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField] private TextAsset _probeComponentConfig, _probeComponentInventoryConfig;
    [SerializeField] private GameObject _player;

    public void Awake()
    {
        List<Tuple<ProbeComponent, int>> startingInventory = new List<Tuple<ProbeComponent, int>>();
        InventoryConfigEntry[] entries = JsonParser.FromJsonArray<InventoryConfigEntry>(_probeComponentInventoryConfig.text);
        foreach (ProbeComponent probeComponent in JsonParser.FromJsonArray<ProbeComponent>(_probeComponentConfig.text))
        {
            foreach (InventoryConfigEntry entry in entries)
            {
                if (entry.ProbeComponentId.Equals(probeComponent.Id))
                {
                    startingInventory.Add(new Tuple<ProbeComponent, int>(probeComponent, entry.Quantity));
                    break;
                }
            }
        }

        _player.GetComponent<Player>().Initialize(startingInventory);
    }

    [Serializable]
    private class InventoryConfigEntry
    {
        public string ProbeComponentId;
        public int Quantity;
    }
}
