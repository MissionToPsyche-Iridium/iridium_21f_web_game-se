using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHelper : MonoBehaviour
{
    [SerializeField]
    private int ColorProfile = 1;

    public static ControlHelper Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }   

    public void ChangeColorProfile(int profile)
    {
        ColorProfile = profile;
        Debug.Log("Color Profile Changed to " + profile);
    }

    public int GetColorProfile()
    {
        return ColorProfile;
    }
}
