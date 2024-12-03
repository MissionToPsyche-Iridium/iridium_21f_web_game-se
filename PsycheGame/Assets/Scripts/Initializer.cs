using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField]
    private TextAsset probeComponentsConfig;
    public void Awake()
    {
        List<Tuple<ProbeComponent, int>> startingInventory = new List<Tuple<ProbeComponent, int>>();
        foreach (ProbeComponent probeComponent in JsonParser.FromJsonArray<ProbeComponent>(probeComponentsConfig.text))
        {
            startingInventory.Add(new Tuple<ProbeComponent, int>(probeComponent, 1));
        }

        Player player = Player.GetInstance();
        player.Initialize(startingInventory);

        BuildManager.GetInstance().Initialize();
    }
}
