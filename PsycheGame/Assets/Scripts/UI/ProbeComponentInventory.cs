using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProbeComponentInventory : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private Image buttonPrefab;

    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            this.AddComponent(null);
        }
    }

    public void AddComponent(Sprite sprite)
    {
        Instantiate(buttonPrefab, content);
    }
}
