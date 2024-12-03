using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private Player() { }

    private static Player _instance;

    public static Player GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Player();
        }
        return _instance;
    }

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
