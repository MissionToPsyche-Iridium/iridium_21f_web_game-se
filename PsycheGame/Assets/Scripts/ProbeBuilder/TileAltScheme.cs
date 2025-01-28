using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TileAltScheme : TileColorScheme
{
    public TileAltScheme()
    {
    }

    public override Color GetColor1()
    {
        return Color.gray;
    }

    public override Color GetColor2()
    {
        return new Color(0.66f, 0.66f, 0.66f);  
    }

    public override Color GetOpenTileColor()
    {
        return Color.blue;
    }

    public override Color GetOccupiedTileColor()
    {
        return new Color(1.0f, 0.65f, 0.0f);  
    }
}