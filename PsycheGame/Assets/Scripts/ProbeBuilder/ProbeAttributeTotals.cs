using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProbeAttributeTotals
{
    public int ScanningRange, FuelCapacity, Speed, Armor, Hp, Weight;

    public ProbeAttributeTotals()
    {
        ScanningRange = 0;
        FuelCapacity = 0;
        Speed = 0;
        Armor = 0;
        Hp = 0;
        Weight = 0;
    }

    public int GetAttributeTotal(ProbeComponentAttribute attribute)
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

    public void SetAttributeTotal(ProbeComponentAttribute attribute, int total)
    {
        switch (attribute)
        {
            case ProbeComponentAttribute.ScanningRange:
                ScanningRange = total;
                break;
            case ProbeComponentAttribute.FuelCapacity:
                FuelCapacity = total;
                break;
            case ProbeComponentAttribute.Speed:
                Speed = total;
                break;
            case ProbeComponentAttribute.Armor:
                Armor = total;
                break;
            case ProbeComponentAttribute.Hp:
                Hp = total;
                break;
            case ProbeComponentAttribute.Weight:
                Weight = total;
                break;
        }
    }

    public void AddToAttributeTotal(ProbeComponentAttribute attribute, int value)
    {
        SetAttributeTotal(attribute, GetAttributeTotal(attribute) + value);
    }
}
