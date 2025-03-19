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

    [SerializeField]
    private TextAsset _maxProbeAttributeValuesConfig;

    private MaxProbeAttributeValues _maxProbeAttributeValues;

    [SerializeField]
    private GameObject _player;

    private Inventory _inventory;
    private List<Tuple<ProbeComponent, GameObject>> _spawned, _undone;

    public void Start()
    {
        _maxProbeAttributeValues = JsonUtilityWrapper.FromJson<MaxProbeAttributeValues>(_maxProbeAttributeValuesConfig.text);

        _inventory = _player.GetComponent<Player>().Inventory;
        _spawned = new List<Tuple<ProbeComponent, GameObject>>();
        _undone = new List<Tuple<ProbeComponent, GameObject>>();
    }

    public List<GameObject> GetSpawnedProbeComponents()
    {
        List<GameObject> probeComponents = new List<GameObject>();
        foreach (Tuple<ProbeComponent, GameObject> tuple in _spawned)
        {
            probeComponents.Add(tuple.Item2);
        }
        return probeComponents;
    }

    public ProbeComponent GetProbeComponentInfo(GameObject probeComponent)
    {
        foreach (Tuple<ProbeComponent, GameObject> tuple in _spawned)
        {
            if (tuple.Item2.Equals(probeComponent))
            {
                return tuple.Item1;
            }
        }
        return null;
    }

    public void SpawnProbeComponent(Tuple<ProbeComponent, GameObject> probeComponentTuple)
    {
        _inventory.RemoveProbeComponent(probeComponentTuple.Item1);
        _spawned.Add(probeComponentTuple);
    }

    public void DespawnProbeComponent(GameObject probeComponent)
    {
        for (int i = 0; i < _spawned.Count; i++)
        {
            Tuple<ProbeComponent, GameObject> tuple = _spawned[i];
            if (tuple.Item2.Equals(probeComponent))
            {
                probeComponent.GetComponent<SpriteDragDrop>().AttemptToRelease();

                _spawned.RemoveAt(i);
                _undone.Add(tuple);

                probeComponent.SetActive(false);

                _inventory.AddProbeComponent(tuple.Item1);

                break;
            }
        }
    }

    public void Undo()
    {
        if (_spawned.Count > 0)
        {
            DespawnProbeComponent(_spawned[_spawned.Count - 1].Item2);
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
            Tuple<ProbeComponent, GameObject> probeComponentTuple = _undone[i];
            if (_inventory.GetProbeComponentQuantity(probeComponentTuple.Item1) > 0 && probeComponentTuple.Item2.GetComponent<SpriteDragDrop>().AttemptToReoccupy())
            {
                _inventory.RemoveProbeComponent(probeComponentTuple.Item1);

                _undone.RemoveAt(i);
                _spawned.Add(probeComponentTuple);

                probeComponentTuple.Item2.SetActive(true);

                return;
            }
            else
            {
                _undone.RemoveAt(i);

                GameObject.Destroy(probeComponentTuple.Item2);
            }
        }
    }

    public ProbeAttributeTotals CalculateAttributeTotals()
    {
        ProbeAttributeTotals totals = new ProbeAttributeTotals();
        foreach (ProbeComponentAttribute attribute in Enum.GetValues(typeof(ProbeComponentAttribute)))
        {
            foreach (Tuple<ProbeComponent, GameObject> tuple in _spawned)
            {
                totals.AddToAttributeTotal(attribute, tuple.Item1.GetAttributeValue(attribute));
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
        foreach (Tuple<ProbeComponent, GameObject> tuple in _spawned)
        {
            creditsUsed += tuple.Item1.Credits;
        }
        return MaxCredits - creditsUsed;
    }

    [Serializable]
    private class MaxProbeAttributeValues
    {
        public int Hp, Armor, FuelCapacity, Speed, ScanningRange;

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
                case ProbeComponentAttribute.Armor:
                    return Armor;
                case ProbeComponentAttribute.Hp:
                    return Hp;
                default:
                    return (int) 1e9;
            }
        }
    }
}