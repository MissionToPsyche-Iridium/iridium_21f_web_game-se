using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using static TileColorScheme;
public class Constants
{
    // placeholder for color scheme until config file is implemented
    // colors schemes:
    // 1 = standard ({green, red} for standard color) **default**
    // 2 = alternate ({blue, orange} for visual accessibility - colorblindness)
    
    public static int ColorScheme = 0;
    // public static int ColorScheme = 2

    public static void SetColorScheme(int scheme)
    {
        ColorScheme = scheme;
    }

    public static int GetColorScheme()
    {
        if (ColorScheme == 0)
        {
            Camera mainCamera = Camera.main;
            GameObject controlHelper = GameObject.Find("ControlHelper");
            ColorScheme = controlHelper.GetComponent<ControlHelper>().GetColorProfile();
            Debug.Log("Color scheme: " + ColorScheme);
        }
        return ColorScheme;
    }

    public static TileColorScheme GetColorSchemeInstance()
    {
        ColorScheme = GameObject.Find("ControlHelper").GetComponent<ControlHelper>().GetColorProfile();
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