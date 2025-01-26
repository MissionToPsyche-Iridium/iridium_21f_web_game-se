using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeComponentBuilder
{
    private string _id, _name, _description;
    private ProbeComponentType _type;
    private int _scanningRange, _fuelCapacity, _speed, _armor, _hp, _weight, _credits, _gridPositionX, _gridPositionY;

    public ProbeComponentBuilder SetId(string id)
    {
        _id = id;
        return this;
    }

    public ProbeComponentBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public ProbeComponentBuilder SetDescription(string description)
    {
        _description = description;
        return this;
    }

    public ProbeComponentBuilder SetType(ProbeComponentType type)
    {
        _type = type;
        return this;
    }

    public ProbeComponentBuilder SetScanningRange(int scanningRange)
    {
        _scanningRange = scanningRange;
        return this;
    }

    public ProbeComponentBuilder SetFuelCapacity(int fuelCapacity)
    {
        _fuelCapacity = fuelCapacity;
        return this;
    }

    public ProbeComponentBuilder SetSpeed(int speed)
    {
        _speed = speed;
        return this;
    }
    public ProbeComponentBuilder SetArmor(int armor)
    {
        _armor = armor;
        return this;
    }
    public ProbeComponentBuilder SetHp(int hp)
    {
        _hp = hp;
        return this;
    }

    public ProbeComponentBuilder SetWeight(int weight)
    {
        _weight = weight;
        return this;
    }

    public ProbeComponentBuilder SetCredits(int credits)
    {
        _credits = credits;
        return this;
    }

    public ProbeComponentBuilder SetGridPosition(int x, int y)
    {
        _gridPositionX = x;
        _gridPositionY = y;
        return this;
    }

    public ProbeComponent Build()
    {
        return new ProbeComponent(
            _id,
            _name,
            _description,
            _type,
            _scanningRange,
            _fuelCapacity,
            _speed,
            _armor,
            _hp,
            _weight,
            _credits,
            _gridPositionX,
            _gridPositionY
        );
    }
}
