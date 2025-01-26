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
    public string Id, Name, Description;
    public ProbeComponentType Type;
    public int ScanningRange, FuelCapacity, Speed, Armor, Hp, Weight, Credits, GridPositionX, GridPositionY;

    public ProbeComponent(
        string id,
        string name,
        string description,
        ProbeComponentType type,
        int scanningRange,
        int fuelCapacity,
        int speed,
        int armor,
        int hp,
        int weight,
        int credits,
        int gridPositionX,
        int gridPositionY
    )
    {
        Id = id;
        Name = name;
        Description = description;
        Type = type;
        ScanningRange = scanningRange;
        FuelCapacity = fuelCapacity;
        Speed = speed;
        Armor = armor;
        Hp = hp;
        Weight = weight;
        Credits = credits;
        GridPositionX = gridPositionX;
        GridPositionY = gridPositionY;
    }
}
