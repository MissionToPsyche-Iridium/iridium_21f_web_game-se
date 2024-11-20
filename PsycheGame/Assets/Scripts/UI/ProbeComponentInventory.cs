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
            if (tuple.Item2.Equals(button))
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

        ProbeComponentButton button = probeComponentButton.GetComponent<ProbeComponentButton>();
        button.ProbeComponentInventory = this;
        button.SpawnArea = _spawnArea;

        Image image = probeComponentButton.GetComponent<Image>();
        image.preserveAspect = true;
        image.sprite = probeComponent.Sprite;

        probeComponentButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = quantity.ToString() + "x";

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
                    if (tuple.Item2.activeSelf)
                    {
                        if (quantity < 1)
                        {
                            tuple.Item2.SetActive(false);
                        }
                    }
                    else if (quantity > 0)
                    {
                        tuple.Item2.SetActive(true);
                    }

                    tuple.Item2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = quantity.ToString() + "x";

                    return;
                }
            }

            CreateButton(probeComponent, quantity);
        }
    }
}
