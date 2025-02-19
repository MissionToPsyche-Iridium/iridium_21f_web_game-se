using UnityEngine;

/*
    Probe Builder:: TileColorScheme.cs
    
    Date: Jan 27, 2024
    Description: this script is a stereotype for the defining the color scheme for the tiles.
*/

public abstract class TileColorScheme
{
    public Color BaseSceneColor { get; set; }
    public float exposure { get; set; }
    public float Threshold { get; set; }
    public int Intensity { get; set; }  
    public Color Tint { get; set; }


    public abstract Color GetColor1();
    public abstract Color GetColor2();
    public abstract Color GetOpenTileColor();
    public abstract Color GetOccupiedTileColor();
}