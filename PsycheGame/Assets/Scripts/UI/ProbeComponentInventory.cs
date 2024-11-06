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
    public const string ButtonResourcePath = "UI/ProbeComponentButton";

    private Inventory _inventory;
    private GameObject _content;
    private GameObject _buttonPrefab;

    public void Initialize(Inventory inventory)
    {
        _inventory = inventory;
        _content = GameObject.Find(InventoryContentPath);
        _buttonPrefab = Resources.Load<GameObject>(ButtonResourcePath);

        foreach (Tuple<ProbeComponent, int> probeComponentTuple in _inventory.GetProbeComponents())
        {
            GameObject probeComponentButton = GameObject.Instantiate(_buttonPrefab);

            Image image = probeComponentButton.GetComponent<Image>();
            image.preserveAspect = true;
            image.sprite = probeComponentTuple.Item1.Sprite;

            probeComponentButton.transform.SetParent(_content.transform);
        }
    }
}
