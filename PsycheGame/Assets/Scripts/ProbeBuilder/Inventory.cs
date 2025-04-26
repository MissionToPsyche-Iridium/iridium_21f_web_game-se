using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory<T>
{
    private List<Entry> _entries;
    private List<IInventoryObserver<T>> _observers;

    public Inventory()
    {
        _entries = new List<Entry>();
        _observers = new List<IInventoryObserver<T>>();
    }

    public List<T> GetItems()
    {
        List<T> items = new List<T>();
        foreach (Entry entry in _entries)
        {
            items.Add(entry.Item);
        }
        return items;
    }

    public int GetItemQuantity(T item)
    {
        foreach (Entry entry in _entries)
        {
            if (entry.Item.Equals(item))
            {
                return entry.Quantity;
            }
        }
        return 0;
    }

    public void AddItem(T item, int startingQuantity = 1)
    {
        _entries.Add(new Entry(item, startingQuantity));
        foreach (IInventoryObserver<T> observer in _observers)
        {
            observer.ItemAdded(item);
        }
    }

    public void RemoveItem(T item)
    {
        for (int entryIndex = 0; entryIndex < _entries.Count; entryIndex++)
        {
            if (_entries[entryIndex].Item.Equals(item))
            {
                _entries.RemoveAt(entryIndex);
                foreach (IInventoryObserver<T> observer in _observers)
                {
                    observer.ItemRemoved(item);
                }

                break;
            }
        }
    }

    public void SetItemQuantity(T item, int quantity)
    {
        foreach (Entry entry in _entries)
        {
            if (entry.Item.Equals(item))
            {
                entry.Quantity = quantity;
                foreach (IInventoryObserver<T> observer in _observers)
                {
                    observer.ItemUpdated(item, quantity);
                }

                break;
            }
        }
    }

    public void IncrementItemQuantity(T item, int increment = 1)
    {
        SetItemQuantity(item, GetItemQuantity(item) + increment);
    }

    public void DecrementItemQuantity(T item, int decrement = 1)
    {
        SetItemQuantity(item, GetItemQuantity(item) - decrement);
    }

    public void AddObserver(IInventoryObserver<T> observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IInventoryObserver<T> observer)
    {
        _observers.Remove(observer);
    }

    private sealed class Entry
    {
        public T Item { get; private set; }
        public int Quantity { get; set; }

        public Entry(T item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}
