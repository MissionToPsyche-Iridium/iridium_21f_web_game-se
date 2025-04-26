using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryObserver<T>
{
    public void ItemAdded(T item);
    public void ItemUpdated(T item, int newQuantity);
    public void ItemRemoved(T item);
}
