using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/*
    Probe Builder :: ProbeComponent.cs
    Date: Mar 20, 2025
    Description: this script defines the probe component class and its attributes. It also contains the enums for the probe component attributes, types, and classes.

    version 1.1 (Mar 20, 2025)
    :: updated the ProbeComponentClass enum type for 'MountType' attribute

*/
public enum ProbeComponentAttribute
{
    ScanningRange,
    FuelCapacity,
    Speed,
    Armor,
    Hp,
    Weight
}

public enum ProbeComponentType
{
    Standard,
    Custom,
    Sensor
}

public enum ProbeComponentMountType
{
    Interior,
    Exterior
}

[Serializable]
public class ProbeComponent
{
    public string Id, Name, Description;
    public ProbeComponentType Type;
    public ProbeComponentMountType MountType;
    public int ScanningRange, FuelCapacity, Speed, Armor, Hp, Weight, GridPositionX, GridPositionY;
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
        int armor,
        int hp,
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
        Armor = armor;
        Hp = hp;
        Weight = weight;
        Credits = credits;
        GridPositionX = gridPositionX;
        GridPositionY = gridPositionY;
    }

    public ProbeComponentMountType GetMountType()
    {
        return MountType;
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
            case ProbeComponentAttribute.Armor:
                return Armor;
            case ProbeComponentAttribute.Hp:
                return Hp;
            case ProbeComponentAttribute.Weight:
                return Weight;
            default:
                return 0;
        }
    }
}
