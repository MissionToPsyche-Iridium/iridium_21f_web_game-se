using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.CompilerServices;

public class ProbeComponentInventory : MonoBehaviour, IInventoryObserver
{
    [SerializeField] private Sprite[] _probeSprites;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private GameObject _spawnArea;
    [SerializeField] private GameObject _filter, _filterLeft, _filterRight;

    private enum FilterType
    {
        All,
        Standard,
        Custom
    }

    private Inventory _inventory;
    private FilterType _currentFilter;
    private List<Tuple<ProbeComponent, GameObject>> _componentButtons;

    public void Start()
    {
        _inventory = Player.GetInstance().Inventory;
        _inventory.AddObserver(this);

        _currentFilter = FilterType.All;

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
        buttonScript.BuildManager = transform.parent.gameObject.GetComponent<BuildManager>();
        buttonScript.ProbeComponent = probeComponent;
        buttonScript.ProbeComponentInventory = this;
        buttonScript.SpawnArea = _spawnArea;

        Image image = button.GetComponent<Image>();
        image.preserveAspect = true;
        foreach (Sprite sprite in _probeSprites)
        {
            if (sprite.name.Equals(probeComponent.Id))
            {
                image.sprite = sprite;
                break;
            }
        }

        if (quantity < 1)
        {
            button.tag = "Inactive";
            image.color = new Color(255, 255, 255, 0.25f);
        }

        probeComponentButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = quantity.ToString() + "x";

        probeComponentButton.transform.SetParent(_content.transform);

        _componentButtons.Add(new Tuple<ProbeComponent, GameObject>(probeComponent, probeComponentButton));
    }

    private void Filter()
    {
        if (_currentFilter == FilterType.All)
        {
            _filter.GetComponent<TextMeshProUGUI>().text = "All";
            _filterLeft.SetActive(false);
            _filterRight.SetActive(true);

            foreach (Tuple<ProbeComponent, GameObject> tuple in _componentButtons)
            {
                tuple.Item2.SetActive(true);
            }
        }
        else
        {
            ProbeComponentType type;
            switch (_currentFilter)
            {
                case FilterType.Custom:
                    type = ProbeComponentType.Custom;

                    _filter.GetComponent<TextMeshProUGUI>().text = "Custom";
                    _filterLeft.SetActive(true);
                    _filterRight.SetActive(false);

                    break;

                default:
                    type = ProbeComponentType.Standard;

                    _filter.GetComponent<TextMeshProUGUI>().text = "Standard";
                    _filterLeft.SetActive(true);
                    _filterRight.SetActive(true);

                    break;
            }

            foreach (Tuple<ProbeComponent, GameObject> tuple in _componentButtons)
            {
                if (tuple.Item1.Type == type)
                {
                    tuple.Item2.SetActive(true);
                }
                else
                {
                    tuple.Item2.SetActive(false);
                }
            }
        }
    }

    public void PreviousFilter()
    {
        if (_currentFilter == FilterType.All)
        {
            return;
        }
        _currentFilter = _currentFilter - 1;
        Filter();
    }

    public void NextFilter()
    {
        if (_currentFilter == FilterType.Custom)
        {
            return;
        }
        _currentFilter = _currentFilter + 1;
        Filter();
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
