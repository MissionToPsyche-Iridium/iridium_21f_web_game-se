using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum ProbeComponentType
{
    Standard,
    Custom
}

[Serializable]
public class ProbeComponent
{
    public string Id;
    public string Name;
    public ProbeComponentType Type;
    public string Description;
    public int ScanningRange;
    public int FuelCapacity;
    public int Speed;
    public int Armor;
    public int Hp;
    public int Weight;
    public int Credits;
    public Position GridPosition; //Not sure if Position is the correct data type for this

    public ProbeComponent(string id, string name, ProbeComponentType type, string description)
    {
        Id = id;
        Type = type;
        Name = name;
        Description = description;

        
    }

    //TODO: create initializer function that sets the initial values based on the name of the probe component.
    //We will need to create a file that stores the initial values for each part and read it in when the game starts
    //I have added this task to taiga 
}
