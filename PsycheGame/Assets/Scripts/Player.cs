using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Inventory inventory;

    void Start()
    {
        inventory = new Inventory();
        foreach (Sprite sprite in Resources.LoadAll("ProbeSprites", typeof(Sprite)))
        {
            inventory.AddProbeComponent(
                new ProbeComponent(
                    sprite.name,
                    "name",
                    "description",
                    0,
                    sprite
                )
            );
        }
        ProbeComponentInventory.GetInstance().Initialize(inventory);
    }
}
