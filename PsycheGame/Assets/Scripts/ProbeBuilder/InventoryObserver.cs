using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryObserver
{
    public void ItemUpdated(object item, int quantity);
}
