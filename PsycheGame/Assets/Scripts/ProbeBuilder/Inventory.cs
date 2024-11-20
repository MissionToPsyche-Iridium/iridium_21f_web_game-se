using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private InventoryContainer<ProbeComponent> _probeComponents;
    private List<IInventoryObserver> _observers;

    public Inventory()
    {
        _probeComponents = new InventoryContainer<ProbeComponent>();
        _observers = new List<IInventoryObserver>();
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
            quantity += _probeComponents.GetItemQuantity(id);
            _probeComponents.UpdateItemQuantity(id, quantity);
        } else
        {
            _probeComponents.AddItem(id, probeComponent, quantity);
        }

        NotifyObservers(probeComponent, quantity);
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
            quantity--;
            _probeComponents.UpdateItemQuantity(id, quantity);
        } else
        {
            quantity = 0;
            _probeComponents.RemoveItem(id);
        }

        NotifyObservers(probeComponent, quantity);
    }

    private void NotifyObservers(object item, int quantity)
    {
        foreach (IInventoryObserver observer in _observers)
        {
            observer.ItemUpdated(item, quantity);
        }
    }

    public void AddObserver(IInventoryObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IInventoryObserver observer)
    {
        _observers.Remove(observer);
    }
}
