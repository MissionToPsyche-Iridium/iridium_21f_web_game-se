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
    [SerializeField] private GameObject _player;

    private Inventory _inventory;
    private List<Tuple<ProbeComponent, GameObject>> _spawned, _undone;

    public void Start()
    {
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

    public void Undo()
    {
        if (_spawned.Count > 0)
        {
            Tuple<ProbeComponent, GameObject> probeComponentTuple = _spawned[_spawned.Count - 1];

            probeComponentTuple.Item2.GetComponent<SpriteDragDrop>().AttemptToRelease();

            _spawned.RemoveAt(_spawned.Count - 1);
            _undone.Add(probeComponentTuple);

            probeComponentTuple.Item2.SetActive(false);

            _inventory.AddProbeComponent(probeComponentTuple.Item1);
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

    public Dictionary<string, int> CalculateAttributeTotals()
    {
        int totalScanningRange = 0,
            totalFuelCapacity = 0,
            totalSpeed = 0,
            totalArmor = 0,
            totalHp = 0,
            totalWeight = 0,
            totalCreditsUsed = 0;

        foreach (Tuple<ProbeComponent, GameObject> tuple in _spawned)
        {
            totalScanningRange += tuple.Item1.ScanningRange;
            totalFuelCapacity += tuple.Item1.FuelCapacity;
            totalSpeed += tuple.Item1.Speed;
            totalArmor += tuple.Item1.Armor;
            totalHp += tuple.Item1.Hp;
            totalWeight += tuple.Item1.Weight;
            totalCreditsUsed += tuple.Item1.Credits;
        }

        Dictionary<string, int> totals = new Dictionary<string, int>();

        totals.Add("ScanningRange", totalScanningRange);
        totals.Add("FuelCapacity", totalFuelCapacity);
        totals.Add("Speed", totalSpeed);
        totals.Add("Armor", totalArmor);
        totals.Add("Hp", totalHp);
        totals.Add("Weight", totalWeight);
        totals.Add("CreditsUsed", totalCreditsUsed);

        return totals;
    }
}