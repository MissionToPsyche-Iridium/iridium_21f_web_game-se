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

public class BuildManager
{
    private BuildManager() { }

    private static BuildManager _instance;

    public static BuildManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BuildManager();
        }
        return _instance;
    }

    private Inventory _inventory;
    private List<Tuple<ProbeComponent, GameObject>> spawned, undone;

    public void Initialize(Inventory inventory)
    {
        _inventory = inventory;
        spawned = new List<Tuple<ProbeComponent, GameObject>>();
        undone = new List<Tuple<ProbeComponent, GameObject>>();
    }

    public List<GameObject> GetSpawnedProbeComponents()
    {
        List<GameObject> probeComponents = new List<GameObject>();
        foreach (Tuple<ProbeComponent, GameObject> tuple in spawned)
        {
            probeComponents.Add(tuple.Item2);
        }
        return probeComponents;
    }

    public ProbeComponent GetProbeComponentInfo(GameObject probeComponent)
    {
        foreach (Tuple<ProbeComponent, GameObject> tuple in spawned)
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
        spawned.Add(probeComponentTuple);
    }

    public void Undo()
    {
        if (spawned.Count > 0)
        {
            Tuple<ProbeComponent, GameObject> probeComponentTuple = spawned[spawned.Count - 1];

            spawned.RemoveAt(spawned.Count - 1);
            undone.Add(probeComponentTuple);

            probeComponentTuple.Item2.SetActive(false);

            _inventory.AddProbeComponent(probeComponentTuple.Item1);
        }
    }

    public void UndoAll()
    {
        for (int i = spawned.Count - 1; i >= 0; i--)
        {
            Tuple<ProbeComponent, GameObject> probeComponentTuple = spawned[i];

            spawned.RemoveAt(i);
            undone.Add(probeComponentTuple);

            probeComponentTuple.Item2.SetActive(false);

            _inventory.AddProbeComponent(probeComponentTuple.Item1);
        }
    }

    public void Redo()
    {
        for (int i = undone.Count - 1; i >= 0; i--)
        {
            Tuple<ProbeComponent, GameObject> probeComponentTuple = undone[i];
            if (_inventory.GetProbeComponentQuantity(probeComponentTuple.Item1) > 0)
            {
                _inventory.RemoveProbeComponent(probeComponentTuple.Item1);

                undone.RemoveAt(i);
                spawned.Add(probeComponentTuple);

                probeComponentTuple.Item2.SetActive(true);

                return;
            }
            else
            {
                undone.RemoveAt(i);

                GameObject.Destroy(probeComponentTuple.Item2);
            }
        }
    }
}