using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InventoryContainer<T>
{
    private List<InventoryItem<T>> _container;

    public InventoryContainer()
    {
        _container = new List<InventoryItem<T>>();
    }

    public T GetItem(string id)
    {
        foreach (InventoryItem<T> item in _container)
        {
            if (item.Id.Equals(id))
            {
                return item.Item;
            }
        }
        return default(T);
    }

    public int GetItemQuantity(string id)
    {
        foreach (InventoryItem<T> item in _container)
        {
            if (item.Id.Equals(id))
            {
                return item.Quantity;
            }
        }
        return 0;
    }

    public List<string> GetItemIds()
    {
        List<string> ids = new List<string>();
        foreach (InventoryItem<T> item in _container)
        {
            ids.Add(item.Id);
        }
        return ids;
    }

    public void AddItem(string id, T item, int quantity)
    {
        _container.Add(new InventoryItem<T>(id, item, quantity));
    }

    public void UpdateItemQuantity(string id, int quantity)
    {
        foreach (InventoryItem<T> item in _container)
        {
            if (item.Id.Equals(id))
            {
                item.Quantity = quantity;
                break;
            }
        }
    }

    public void RemoveItem(string id)
    {
        for (int index = 0; index < _container.Count; index++)
        {
            if (_container[index].Id.Equals(id))
            {
                _container.RemoveAt(index);
                break;
            }
        }
    }
}
