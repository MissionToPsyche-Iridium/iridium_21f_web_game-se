using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ProbeComponentInventory : MonoBehaviour
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

        _componentButtons = new List<Tuple<ProbeComponent, GameObject>>();

        foreach (Tuple<ProbeComponent, int> tuple in _inventory.GetProbeComponentQuantities())
        {
            GameObject probeComponentButton = GameObject.Instantiate(_buttonPrefab);
            ProbeComponentButton button = probeComponentButton.GetComponent<ProbeComponentButton>();
            button.ProbeComponentInventory = this;
            button.SpawnArea = _spawnArea;

            Image image = probeComponentButton.GetComponent<Image>();
            image.preserveAspect = true;
            image.sprite = tuple.Item1.Sprite;
            image.material = Resources.Load<Material>("default-particle");

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
