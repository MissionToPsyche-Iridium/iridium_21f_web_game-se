using UnityEngine;
using static TileColorScheme;
public class Constants
{
    // placeholder for color scheme until config file is implemented
    // colors scheme 1 = standard, 2 = alternate
    
    // public static int ColorScheme = 1;
    public static int ColorScheme = 2;

    public static void SetColorScheme(int scheme)
    {
        ColorScheme = scheme;
    }

    public static int GetColorScheme()
    {
        return ColorScheme;
    }

    public static TileColorScheme GetColorSchemeInstance()
    {
        if (ColorScheme == 1)
        {
            return new TileStdScheme();
        }
        else
        {
            Debug.Log("Using alternate color scheme");
            return new TileAltScheme();
        }
    }
}