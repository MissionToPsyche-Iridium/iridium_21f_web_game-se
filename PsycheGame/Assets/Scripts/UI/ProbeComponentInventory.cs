using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.CompilerServices;
using System.Linq;

public class ProbeComponentInventory : MonoBehaviour, IInventoryObserver<ProbeComponent>
{
    [SerializeField] private Sprite[] _probeSprites;
    [SerializeField] private GameObject _foregroundCanvas;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private GameObject _tooltipPrefab;
    [SerializeField] private GameObject _notificationPrefab;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _infoPartName, _infoPartDescription, _infoPartCredits, _infoPartImage;
    [SerializeField] private GameObject _spawnArea;
    [SerializeField] private GameObject _filter, _filterLeft, _filterRight;
    [SerializeField] private GameObject _draggingBox;

    public Inventory<ProbeComponent> Inventory { get; private set; }

    private ProbeComponentType? _currentFilter;
    private List<ProbeComponentButton> _componentButtons;

    public void Awake()
    {
        Inventory = new Inventory<ProbeComponent>();
        foreach (ProbeComponent probeComponent in Config.Get<ProbeComponent[]>("ProbeComponents"))
        {
            for (int i = 0; i < Config.Get<int>("#StartingInventory"); i++)
            {
                if (Config.Get<string>($"StartingInventory[{i}].ProbeComponentId").Equals(probeComponent.Id))
                {
                    Inventory.AddItem(probeComponent, Config.Get<int>($"StartingInventory[{i}].Quantity"));
                }
            }
        }
        Inventory.AddObserver(this);

        _currentFilter = null;

        _componentButtons = new List<ProbeComponentButton>();

        foreach (ProbeComponent probeComponent in Inventory.GetItems())
        {
            CreateButton(probeComponent, Inventory.GetItemQuantity(probeComponent));
        }
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
        buttonScript.DraggingBox = _draggingBox;
        buttonScript.InfoPanel = _infoPanel;
        buttonScript.InfoPartName = _infoPartName;
        buttonScript.InfoPartDescription = _infoPartDescription;
        buttonScript.InfoPartCredits = _infoPartCredits;
        buttonScript.InfoPartImage = _infoPartImage;
        buttonScript.SpawnArea = _spawnArea;
        buttonScript.TooltipPrefab = _tooltipPrefab;
        buttonScript.NotificationPrefab = _notificationPrefab;
        buttonScript.MasterCanvas = transform.parent.gameObject;
        buttonScript.ForegroundCanvas = _foregroundCanvas;

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

        probeComponentButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"{quantity.ToString()}x";

        probeComponentButton.transform.SetParent(_content.transform);

        _componentButtons.Add(buttonScript);
    }

    private void Filter()
    {
        if (_currentFilter == null)
        {
            _filter.GetComponent<TextMeshProUGUI>().text = "All";
            foreach (ProbeComponentButton button in _componentButtons)
            {
                button.transform.parent.gameObject.SetActive(true);
            }
        }
        else
        {
            _filter.GetComponent<TextMeshProUGUI>().text = _currentFilter.ToString();
            foreach (ProbeComponentButton button in _componentButtons)
            {
                if (button.ProbeComponent.Type == _currentFilter)
                {
                    button.transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    button.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }

    public void PreviousFilter()
    {
        IEnumerable<ProbeComponentType> types = Enum.GetValues(typeof(ProbeComponentType)).Cast<ProbeComponentType>();
        _currentFilter = _currentFilter == null
            ? types.Max()
            : (_currentFilter == types.Min()
                ? null
                : _currentFilter - 1);
        Filter();
    }

    public void NextFilter()
    {
        IEnumerable<ProbeComponentType> types = Enum.GetValues(typeof(ProbeComponentType)).Cast<ProbeComponentType>();
        _currentFilter = _currentFilter == types.Max()
            ? null
            : (_currentFilter == null
                ? types.Min()
                : _currentFilter + 1);
        Filter();
    }

    public void ItemAdded(ProbeComponent probeComponent)
    {
        CreateButton(probeComponent, Inventory.GetItemQuantity(probeComponent));
    }

    public void ItemUpdated(ProbeComponent probeComponent, int newQuantity)
    {
        if (probeComponent != null)
        {
            foreach (ProbeComponentButton button in _componentButtons)
            {
                if (button.ProbeComponent.Equals(probeComponent))
                {
                    if (!button.gameObject.tag.Equals("Inactive"))
                    {
                        if (newQuantity < 1)
                        {
                            button.tag = "Inactive";
                            button.GetComponent<Image>().color = new Color(255, 255, 255, 0.25f);
                        }
                    }
                    else if (newQuantity > 0)
                    {
                        button.tag = "Untagged";
                        button.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                    }

                    button.transform.parent.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"{newQuantity.ToString()}x";
                }
            }
        }
    }

    public void ItemRemoved(ProbeComponent probeComponent)
    {
        for (int buttonIndex = 0; buttonIndex < _componentButtons.Count; buttonIndex++)
        {
            if (_componentButtons[buttonIndex].ProbeComponent.Equals(probeComponent))
            {
                Destroy(_componentButtons[buttonIndex].transform.parent.gameObject);
                _componentButtons.RemoveAt(buttonIndex);
                break;
            }
        }
    }
}