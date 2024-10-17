using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProbeComponentInventory : MonoBehaviour
{
    [SerializeField] private Transform panel; 

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            this.AddComponent(null);
        }
    }

    void Update()
    {

    }

    public void AddComponent(Sprite sprite)
    {
        GameObject component = new GameObject();
        Image componentImage = component.AddComponent<Image>();
        componentImage.sprite = sprite;
        component.transform.SetParent(panel.transform);
    }
}
