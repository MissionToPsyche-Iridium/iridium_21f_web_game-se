using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Sprite[] _probeSprites;

    public Inventory Inventory { get; private set; }

    public void Awake()
    {
        Inventory = new Inventory();
        foreach (Sprite sprite in _probeSprites)
        {
            Inventory.AddProbeComponent(
                new ProbeComponent(
                    "name",
                    "description",
                    sprite
                )
            );
        }
    }

    public void Start()
    {
        BuildManager.GetInstance().Initialize(Inventory);
    }
}
