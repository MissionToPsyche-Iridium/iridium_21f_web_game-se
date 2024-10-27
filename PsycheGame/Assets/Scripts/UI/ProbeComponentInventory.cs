using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProbeComponentInventory
{
    private ProbeComponentInventory() { }

    private static ProbeComponentInventory _instance;

    public static ProbeComponentInventory GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ProbeComponentInventory();
        }
        return _instance;
    }

    public const string InventoryContentPath = "/BuildManager/MasterCanvas/InventoryCanvas/BackgroundPanel/ComponentPanel/Viewport/Content";

    private Inventory _inventory;
    private GameObject _content;

    public void Initialize(Inventory inventory)
    {
        _inventory = inventory;
        _content = GameObject.Find(InventoryContentPath);

        foreach (Tuple<ProbeComponent, int> probeComponent in _inventory.GetProbeComponents())
        {
            // TODO
        }
    }
}
