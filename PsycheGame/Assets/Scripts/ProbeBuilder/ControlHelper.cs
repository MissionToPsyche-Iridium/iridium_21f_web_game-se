using UnityEngine;

/*
    ControlHelper.cs
    Description: This script manages the color profile for the game, allowing for dynamic changes and persistence across scenes.

    v1.1: Apr 16, 2025
    :: reviewed and updated the code to make script a singleton and ensure only one instance exists
*/
public class ControlHelper : MonoBehaviour
{
    [SerializeField]
    private int colorProfile = 0;   

    public static ControlHelper Instance { get; private set; }

    private void Awake()
    {
        if (colorProfile == 0)
        {
            Debug.LogWarning("Color Profile is not set. Defaulting to 1.");
            colorProfile = 1; 
        }
        
        // Make control helper singleton - persist through scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeColorProfile(int profile)
    {
        if (profile < 0) 
        {
            Debug.LogWarning("Invalid Color Profile value: " + profile);
            return;
        }

        colorProfile = profile;
        Debug.Log("Color Profile Changed to " + profile);
    }

    public int GetColorProfile()
    {
        return colorProfile;
    }
}
