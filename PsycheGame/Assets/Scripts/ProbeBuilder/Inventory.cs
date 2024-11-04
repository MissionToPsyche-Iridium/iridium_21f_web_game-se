using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InventoryContainer<T>
{
    private List<ProbeComponent> _container;

    public InventoryContainer()
    {
        _container = new List<ProbeComponent>();
    }

    public ProbeComponent GetItem(string id)
    {
        foreach (ProbeComponent item in _container)
        {
            if (item.getId().Equals(id))
            {
                return item;
            }
        }
        return null;
    }

    public int GetItemQuantity(string id)
    {
        foreach (ProbeComponent item in _container)
        {
            if (item.getId().Equals(id))
            {
                return item.getQuantity();
            }
        }
        return 0;
    }

    public List<int> GetItemIds()
    {
        List<int> ids = new List<int>();
        foreach (ProbeComponent item in _container)
        {
            ids.Add(item.getId());
        }
        return ids;
    }

    public void AddItem(ProbeComponent probeComponent)
    {
        _container.Add(probeComponent);
    }

    public void UpdateItemQuantity(string id, int quantity)
    {
        foreach (ProbeComponent item in _container)
        {
            if (item.getId().Equals(id))
            {
                item.setQuantity(quantity);
                break;
            }
        }
    }

    public void RemoveItem(ProbeComponent probeComponent)
    {
        for (int index = 0; index < _container.Count; index++)
        {
            if (_container[index].getId().Equals(probeComponent.getId()))
            {
                _container.RemoveAt(index);
                break;
            }
        }
    }
}