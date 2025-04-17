using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum ProbeComponentAttribute
{
    ScanningRange,
    FuelCapacity,
    Speed,
    Health
}

public enum ProbeComponentType
{
    Sensor,
    Movement,
    Power,
    Communication,
    Tools,
    Other
}

public enum ProbeComponentMountType
{
    Any,
    Interior,
    Exterior
}

[Serializable]
public class ProbeComponent
{
    public string Id, Name, Description;
    public ProbeComponentType Type;
    public ProbeComponentMountType MountType;
    public int ScanningRange, FuelCapacity, Speed, Health, Weight, GridPositionX, GridPositionY;
    public float Credits;

    public ProbeComponent(
        string id,
        string name,
        string description,
        ProbeComponentType type,
        ProbeComponentMountType mountType,
        int scanningRange,
        int fuelCapacity,
        int speed,
        int health,
        int weight,
        float credits,
        int gridPositionX,
        int gridPositionY
    )
    {
        Id = id;
        Name = name;
        Description = description;
        Type = type;
        MountType = mountType;
        ScanningRange = scanningRange;
        FuelCapacity = fuelCapacity;
        Speed = speed;
        Health = health;
        Weight = weight;
        Credits = credits;
        GridPositionX = gridPositionX;
        GridPositionY = gridPositionY;
    }

    public int GetAttributeValue(ProbeComponentAttribute attribute)
    {
        switch (attribute)
        {
            case ProbeComponentAttribute.ScanningRange:
                return ScanningRange;
            case ProbeComponentAttribute.FuelCapacity:
                return FuelCapacity;
            case ProbeComponentAttribute.Speed:
                return Speed;
            case ProbeComponentAttribute.Health:
                return Health;
            default:
                return 0;
        }
    }
}
