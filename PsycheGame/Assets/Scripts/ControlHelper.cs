using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlHelper : MonoBehaviour
{
    [SerializeField]
    private int ColorProfile = 1;
    // Start is called before the first frame update

    public void ChangeColorProfile(int profile)
    {
        ColorProfile = profile;
        Debug.Log("Color Profile Changed to " + profile);
    }

    public int GetColorProfile()
    {
        return ColorProfile;
    }
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
    }
}
