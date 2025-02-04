using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    Probe Builder:: TileStdScheme.cs
    
    Date: Jan 27, 2024
    Description: this script is a stereotype that implements the abstract class TileColorScheme.  It defines the standard color scheme for the tiles.

    Scheme: 1 {open = green, occupied = red, color1 = gray, color2 = light gray}

*/

public class TileStdScheme : TileColorScheme
{
    public TileStdScheme()
    {
        BaseSceneColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        exposure = 0.5f;
        Threshold = 0.995f;
        Intensity = 10;
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
        return Color.green;
    }

    public override Color GetOccupiedTileColor()
    {
        return Color.red;
    }
}