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
        foreach (ProbeComponent probeComponent in Config.Get<ProbeComponent[]>("ProbeComponents"))
        {
            for (int i = 0; i < Config.Get<int>("#StartingInventory"); i++)
            {
                if (Config.Get<string>($"StartingInventory[{i}].ProbeComponentId").Equals(probeComponent.Id))
                {
                    startingInventory.Add(new Tuple<ProbeComponent, int>(
                        probeComponent,
                        Config.Get<int>($"StartingInventory[{i}].Quantity")
                    ));
                }
            }
        }

        _player.GetComponent<Player>().Initialize(startingInventory);
    }
}
