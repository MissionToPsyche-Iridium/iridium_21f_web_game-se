using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private InventoryContainer<ProbeComponent> _probeComponents;

    public Inventory()
    {
        _probeComponents = new InventoryContainer<ProbeComponent>();
    }

    public List<Tuple<ProbeComponent, int>> GetProbeComponentQuantities()
    {
        List<Tuple<ProbeComponent, int>> probeComponents = new List<Tuple<ProbeComponent, int>>();
        foreach (string id in _probeComponents.GetItemIds())
        {
            probeComponents.Add(new Tuple<ProbeComponent, int>(_probeComponents.GetItem(id), _probeComponents.GetItemQuantity(id)));
        }
        return probeComponents;
    }

    public int GetProbeComponentQuantity(ProbeComponent probeComponent)
    {
        return _probeComponents.GetItemQuantity(probeComponent.Id);
    }

    public void AddProbeComponent(ProbeComponent probeComponent, int quantity)
    {
        string id = probeComponent.Id;
        if (_probeComponents.GetItem(id) != null)
        {
            _probeComponents.UpdateItemQuantity(id, _probeComponents.GetItemQuantity(id) + quantity);
            return;
        }
        _probeComponents.AddItem(id, probeComponent, quantity);
    }

    public void AddProbeComponent(ProbeComponent probeComponent)
    {
        AddProbeComponent(probeComponent, 1);
    }

    public void RemoveProbeComponent(ProbeComponent probeComponent)
    {
        string id = probeComponent.Id;
        int quantity = _probeComponents.GetItemQuantity(id);
        if (quantity > 1)
        {
            _probeComponents.UpdateItemQuantity(id, quantity - 1);
            return;
        }
        _probeComponents.RemoveItem(id);
    }
}
