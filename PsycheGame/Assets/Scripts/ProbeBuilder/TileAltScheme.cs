using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Probe Builder:: TileAltScheme.cs
    
    Date: Jan 27, 2024
    Description: this script is a stereotype that implements the abstract class TileColorScheme.  It defines the alternate color scheme for the tiles.

    Scheme: 2 {open = blue, occupied = orange, color1 = gray, color2 = light gray}
*/

public class TileAltScheme : TileColorScheme
{

    public TileAltScheme()
    {
        BaseSceneColor = new Color(0.0f, 0.7f, 1.13f, 0.6f);
        exposure = 0.65f;
        Threshold = 0.995f;
        Intensity = 12;
        Tint = new Color(0.0f, 0.0f, 0.0f, 0.0f);
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