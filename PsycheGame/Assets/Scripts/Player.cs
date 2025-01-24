using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory Inventory { get; private set; }

    public void Initialize(List<Tuple<ProbeComponent, int>> startingInventory)
    {
        Inventory = new Inventory();
        foreach (Tuple<ProbeComponent, int> tuple in startingInventory)
        {
            Inventory.AddProbeComponent(tuple.Item1, tuple.Item2);
        }
    }
}
