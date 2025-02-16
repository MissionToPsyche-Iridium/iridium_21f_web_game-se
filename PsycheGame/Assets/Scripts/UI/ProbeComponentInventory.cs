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
    [SerializeField] private GameObject _player;
    [SerializeField] private Sprite[] _probeSprites;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private GameObject _tooltipPrefab;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _infoPartName, _infoPartDescription, _infoPartImage;
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
        _inventory = _player.GetComponent<Player>().Inventory;
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
        buttonScript.ComponentPanel = transform.GetChild(0).GetChild(0).gameObject;
        buttonScript.InfoPanel = _infoPanel;
        buttonScript.InfoPartName = _infoPartName;
        buttonScript.InfoPartDescription = _infoPartDescription;
        buttonScript.InfoPartImage = _infoPartImage;
        buttonScript.SpawnArea = _spawnArea;
        buttonScript.TooltipPrefab = _tooltipPrefab;
        buttonScript.MasterCanvas = transform.parent.gameObject;

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

        probeComponentButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"{quantity.ToString()}x - {probeComponent.Credits} Credits";

        probeComponentButton.transform.SetParent(_content.transform);

        _componentButtons.Add(new Tuple<ProbeComponent, GameObject>(probeComponent, probeComponentButton));
    }

    private void Filter()
    {
        if (_currentFilter == FilterType.All)
        {
            _filter.GetComponent<TextMeshProUGUI>().text = "All";
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
                    break;

                default:
                    type = ProbeComponentType.Standard;
                    _filter.GetComponent<TextMeshProUGUI>().text = "Standard";
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
        _currentFilter = _currentFilter == FilterType.All ? FilterType.Custom : _currentFilter - 1;
        Filter();
    }

    public void NextFilter()
    {
        _currentFilter = _currentFilter == FilterType.Custom ? FilterType.All : _currentFilter + 1;
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

                    tuple.Item2.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"{quantity.ToString()}x - {probeComponent.Credits} Credits";

                    return;
                }
            }

            CreateButton(probeComponent, quantity);
        }
    }
}