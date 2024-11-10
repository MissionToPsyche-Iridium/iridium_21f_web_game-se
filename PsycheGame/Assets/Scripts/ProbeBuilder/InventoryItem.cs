using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem<T>
{
    public string Id { get; private set; }
    public T Item { get; private set; }
    public int Quantity { get; set; }

    public InventoryItem(string id, T item, int quantity)
    {
        Id = id;
        Item = item;
        Quantity = quantity;
    }
}
