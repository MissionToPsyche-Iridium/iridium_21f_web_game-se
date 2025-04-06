using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProbeAttributeTotals
{
    public int ScanningRange, FuelCapacity, Speed, Health;

    public ProbeAttributeTotals()
    {
        ScanningRange = 0;
        FuelCapacity = 0;
        Speed = 0;
        Health = 0;
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
            case ProbeComponentAttribute.Health:
                return Health;
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
            case ProbeComponentAttribute.Health:
                Health = total;
                break;
        }
    }

    public void AddToAttributeTotal(ProbeComponentAttribute attribute, int value)
    {
        SetAttributeTotal(attribute, GetAttributeTotal(attribute) + value);
    }
}
