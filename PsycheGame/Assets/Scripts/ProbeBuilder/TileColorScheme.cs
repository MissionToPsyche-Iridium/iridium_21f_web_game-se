using UnityEngine;

public abstract class TileColorScheme : MonoBehaviour
{
    public abstract Color GetColor1();
    public abstract Color GetColor2();
    public abstract Color GetOpenTileColor();
    public abstract Color GetOccupiedTileColor();
}