using UnityEngine;

/*
    Probe Builder:: TileColorScheme.cs
    Date: Jan 27, 2024
    Description: this script is a stereotype for the defining the color scheme for the tiles.

    note: reviewed code on Apr 16, 2025.  no functional changes, cleaned up comments and formatting on 
    this abstract class and the derived classes TileStdScheme and TileAltScheme classes.
*/

public abstract class TileColorScheme
{
    public Color BaseSceneColor { get; set; }
    public float Exposure { get; set; }
    public float Threshold { get; set; }
    public int Intensity { get; set; }
    public Color Tint { get; set; }
    public Color AttribBarColor { get; set; }


    public abstract Color GetColor1();
    public abstract Color GetColor2();
    public abstract Color GetOpenTileColor();
    public abstract Color GetOccupiedTileColor();
    public abstract Color GetAttribBarColor();
}