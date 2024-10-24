using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<Tuple<ProbeComponent, int>> ProbeComponents { get; }

    public Inventory()
    {
        ProbeComponents = new List<Tuple<ProbeComponent, int>>();
    }

    public void AddComponent(ProbeComponent probeComponent, int quantity)
    {
        ProbeComponents.Add(new Tuple<ProbeComponent, int>(probeComponent, quantity));
    }
}
