using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/**
 * BuildManager.cs
 * 
 * This class manages the state of probe assembly. Specifically, it manages a list of all spawned probe
 * components. It also implements the undo/redo functionality.
 */

public class BuildManager : MonoBehaviour
{
    private const float MaxCredits = 1000.0f;

    private MaxProbeAttributeValues _maxProbeAttributeValues;

    [SerializeField]
    private ProbeComponentInventory _probeComponentInventory;
    [SerializeField]
    private AttributeTracker _attributeTracker;

    private Inventory<ProbeComponent> _inventory;
    private List<SpriteDragDrop> _spawned, _undone;

    public void Start()
    {
        _maxProbeAttributeValues = new MaxProbeAttributeValues();
        _maxProbeAttributeValues.Health = Config.Get<int>("MaxAttributes.Health");
        _maxProbeAttributeValues.FuelCapacity = Config.Get<int>("MaxAttributes.FuelCapacity");
        _maxProbeAttributeValues.Speed = Config.Get<int>("MaxAttributes.Speed");
        _maxProbeAttributeValues.ScanningRange = Config.Get<int>("MaxAttributes.ScanningRange");

        _inventory = _probeComponentInventory.Inventory;
        _spawned = new List<SpriteDragDrop>();
        _undone = new List<SpriteDragDrop>();
    }

    public List<GameObject> GetSpawnedProbeComponents()
    {
        List<GameObject> probeComponents = new List<GameObject>();
        foreach (SpriteDragDrop instance in _spawned)
        {
            probeComponents.Add(instance.gameObject);
        }
        return probeComponents;
    }

    public ProbeComponent GetProbeComponentInfo(GameObject probeComponent)
    {
        foreach (SpriteDragDrop instance in _spawned)
        {
            if (instance.gameObject.Equals(probeComponent))
            {
                return instance.ProbeComponent;
            }
        }
        return null;
    }

    public void SpawnProbeComponent(SpriteDragDrop instance)
    {
        _inventory.DecrementItemQuantity(instance.ProbeComponent);
        _spawned.Add(instance);
        _attributeTracker.UpdatePanel();
    }

    public void DespawnProbeComponent(GameObject probeComponent)
    {
        for (int i = 0; i < _spawned.Count; i++)
        {
            SpriteDragDrop instance = _spawned[i];
            if (instance.gameObject.Equals(probeComponent))
            {
                instance.AttemptToRelease();

                _spawned.RemoveAt(i);
                _undone.Add(instance);

                probeComponent.SetActive(false);

                _inventory.IncrementItemQuantity(instance.ProbeComponent);

                break;
            }
        }
        _attributeTracker.UpdatePanel();
    }

    public void Undo()
    {
        if (_spawned.Count > 0)
        {
            DespawnProbeComponent(_spawned[_spawned.Count - 1].gameObject);
        }
    }

    public void UndoAll()
    {
        for (int i = _spawned.Count - 1; i >= 0; i--)
        {
            Undo();
        }
    }

    public void Redo()
    {
        for (int i = _undone.Count - 1; i >= 0; i--)
        {
            SpriteDragDrop instance = _undone[i];
            if (_inventory.GetItemQuantity(instance.ProbeComponent) > 0 && instance.AttemptToReoccupy())
            {
                _inventory.DecrementItemQuantity(instance.ProbeComponent);

                _undone.RemoveAt(i);
                _spawned.Add(instance);

                instance.gameObject.SetActive(true);

                return;
            }
            else
            {
                _undone.RemoveAt(i);

                GameObject.Destroy(instance.gameObject);
            }
        }
    }

    public ProbeAttributeTotals CalculateAttributeTotals()
    {
        ProbeAttributeTotals totals = new ProbeAttributeTotals();
        foreach (ProbeComponentAttribute attribute in Enum.GetValues(typeof(ProbeComponentAttribute)))
        {
            foreach (SpriteDragDrop instance in _spawned)
            {
                totals.AddToAttributeTotal(attribute, instance.ProbeComponent.GetAttributeValue(attribute));
            }
            totals.SetAttributeTotal(attribute, Math.Min(totals.GetAttributeTotal(attribute), GetAttributeMaxValue(attribute)));
        }
        return totals;
    }

    public int GetAttributeMaxValue(ProbeComponentAttribute attribute)
    {
        return _maxProbeAttributeValues.GetAttributeMaxValue(attribute);
    }

    public float GetAvailableCredits()
    {
        float creditsUsed = 0.0f;
        foreach (SpriteDragDrop instance in _spawned)
        {
            creditsUsed += instance.ProbeComponent.Credits;
        }
        return MaxCredits - creditsUsed;
    }

    [Serializable]
    private class MaxProbeAttributeValues
    {
        public int Health, FuelCapacity, Speed, ScanningRange;

        public int GetAttributeMaxValue(ProbeComponentAttribute attribute)
        {
            switch (attribute)
            {
                case ProbeComponentAttribute.ScanningRange:
                    return ScanningRange;
                case ProbeComponentAttribute.FuelCapacity:
                    return FuelCapacity;
                case ProbeComponentAttribute.Speed:
                    return Speed;
                case ProbeComponentAttribute.Health:
                    return Health;
                default:
                    return (int) 1e9;
            }
        }
    }
}