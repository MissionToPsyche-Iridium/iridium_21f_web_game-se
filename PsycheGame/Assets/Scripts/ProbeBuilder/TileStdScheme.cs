using UnityEngine;
public class TileStdScheme : TileColorScheme
{
    public TileStdScheme()
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
        return Color.green;
    }

    public override Color GetOccupiedTileColor()
    {
        return Color.red;
    }
}