using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ProbeComponentInventory : MonoBehaviour, IInventoryObserver
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private GameObject _spawnArea;

    private Inventory _inventory;
    private List<Tuple<ProbeComponent, GameObject>> _componentButtons;

    public void Start()
    {
        _inventory = _player.GetComponent<Player>().Inventory;
        _inventory.AddObserver(this);

        _componentButtons = new List<Tuple<ProbeComponent, GameObject>>();

        foreach (Tuple<ProbeComponent, int> tuple in _inventory.GetProbeComponentQuantities())
        {
            CreateButton(tuple.Item1, tuple.Item2);
        }
    }

    public ProbeComponent GetProbeComponent(GameObject button)
    {
        foreach (Tuple<ProbeComponent, GameObject> tuple in _componentButtons)
        {
            if (tuple.Item2.transform.GetChild(0).gameObject.Equals(button))
            {
                return tuple.Item1;
            }
        }
        return null;
    }

    public void CreateButton(ProbeComponent probeComponent, int quantity)
    {
        GameObject probeComponentButton = GameObject.Instantiate(_buttonPrefab);
        probeComponentButton.name = probeComponent.Name;

        GameObject button = probeComponentButton.transform.GetChild(0).gameObject;

        ProbeComponentButton buttonScript = button.GetComponent<ProbeComponentButton>();
        buttonScript.ProbeComponent = probeComponent;
        buttonScript.ProbeComponentInventory = this;
        buttonScript.SpawnArea = _spawnArea;

        Image image = button.GetComponent<Image>();
        image.preserveAspect = true;
        image.sprite = probeComponent.Sprite;

        if (quantity < 1)
        {
            button.tag = "Inactive";
            image.color = new Color(255, 255, 255, 0.5f);
        }

        probeComponentButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = quantity.ToString() + "x";

        probeComponentButton.transform.SetParent(_content.transform);

        _componentButtons.Add(new Tuple<ProbeComponent, GameObject>(probeComponent, probeComponentButton));
    }

    public void ItemUpdated(object item, int quantity)
    {
        ProbeComponent probeComponent = item as ProbeComponent;
        if (probeComponent != null)
        {
            foreach (Tuple<ProbeComponent, GameObject> tuple in _componentButtons)
            {
                if (tuple.Item1.Equals(probeComponent))
                {
                    GameObject button = tuple.Item2.transform.GetChild(0).gameObject;
                    if (!button.tag.Equals("Inactive"))
                    {
                        if (quantity < 1)
                        {
                            button.tag = "Inactive";
                            button.GetComponent<Image>().color = new Color(255, 255, 255, 0.25f);
                        }
                    }
                    else if (quantity > 0)
                    {
                        button.tag = "Untagged";
                        button.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                    }

                    tuple.Item2.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = quantity.ToString() + "x";

                    return;
                }
            }

            CreateButton(probeComponent, quantity);
        }
    }
}
