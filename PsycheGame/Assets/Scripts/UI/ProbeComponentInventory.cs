using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    private List<Tuple<ProbeComponent, GameObject>> _componentButtons;

    public void Initialize(Inventory inventory)
    {
        _inventory = inventory;
        _content = GameObject.Find(InventoryContentPath);
        _buttonPrefab = Resources.Load<GameObject>(ButtonResourcePath);

        _componentButtons = new List<Tuple<ProbeComponent, GameObject>>();

        foreach (Tuple<ProbeComponent, int> tuple in _inventory.GetProbeComponentQuantities())
        {
            GameObject probeComponentButton = GameObject.Instantiate(_buttonPrefab);

            Image image = probeComponentButton.GetComponent<Image>();
            image.preserveAspect = true;
            image.sprite = tuple.Item1.Sprite;

            probeComponentButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = tuple.Item2.ToString() + "x";

            probeComponentButton.transform.SetParent(_content.transform);

            _componentButtons.Add(new Tuple<ProbeComponent, GameObject>(tuple.Item1, probeComponentButton));
        }
    }

    public ProbeComponent GetProbeComponent(GameObject button)
    {
        foreach (Tuple<ProbeComponent, GameObject> tuple in _componentButtons)
        {
            if (tuple.Item2.Equals(button))
            {
                return tuple.Item1;
            }
        }
        return null;
    }

    public void ProbeComponentUsed(ProbeComponent probeComponent)
    {
        _inventory.RemoveProbeComponent(probeComponent);

        foreach (Tuple<ProbeComponent, GameObject> tuple in _componentButtons)
        {
            if (tuple.Item1.Equals(probeComponent))
            {
                int quantity = _inventory.GetProbeComponentQuantity(probeComponent);
                if (quantity < 1)
                {
                    tuple.Item2.SetActive(false);
                }

                tuple.Item2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = quantity.ToString() + "x";

                break;
            }
        }
    }
}
