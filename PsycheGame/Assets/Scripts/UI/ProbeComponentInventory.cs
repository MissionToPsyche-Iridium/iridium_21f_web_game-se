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

        foreach (Tuple<ProbeComponent, int> probeComponentTuple in _inventory.GetProbeComponents())
        {
            GameObject probeComponentButton = new GameObject();

            Image image = probeComponentButton.AddComponent<Image>();
            image.preserveAspect = true;
            image.sprite = probeComponentTuple.Item1.Sprite;

            Button button = probeComponentButton.AddComponent<Button>();
            button.onClick.AddListener(OnClick);

            probeComponentButton.transform.SetParent(_content.transform);
        }
    }

    public void OnClick()
    {

    }
}
