using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProbeComponentInventory : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private Image buttonPrefab;

    public ProbeComponentInventory(Inventory inventory)
    {
        foreach (Tuple<ProbeComponent, int> component in inventory.ProbeComponents)
        {
            Image button = Instantiate(buttonPrefab, content);
            button.sprite = component.Item1.Sprite;
        }
    }
}
