using UnityEngine;
using static TileColorScheme;
public class Constants
{
    // placeholder for color scheme until config file is implemented
    // colors schemes:
    // 1 = standard ({green, red} for standard color) **default**
    // 2 = alternate ({blue, orange} for visual accessibility - colorblindness)
    
<<<<<<< HEAD
    // public static int ColorScheme = 1;
    public static int ColorScheme = 0;
=======
    public static int ColorScheme = 1;
    // public static int ColorScheme = 2;
>>>>>>> US#239-scene-color-scheme

    public static void SetColorScheme(int scheme)
    {
        ColorScheme = scheme;
    }

    public static int GetColorScheme()
    {
        if (ColorScheme == 0)
        {
            GameObject controlHelper = GameObject.Find("Main Camera");
            ColorScheme = controlHelper.GetComponent<ControlHelper>().GetColorProfile();
        }
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