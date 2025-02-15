using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*

    ColorBlind Mode Toggler :: CBToggler.cs
    Date: Jan, 2024
    Description: this script provides the functionality to toggle the colorblind mode on and off. It changes the color scheme of the game
    based on the toggle state.

    version 1.1 (Feb 11)
    :: updated logic to work with different scenes -- ex: either from main menu or applied directly in the builder scene
*/

public class CBToggler : MonoBehaviour
{
    public Toggle cbt_toggle;
    public GameObject controlHelper;
    Camera mainCamera;

    void Start()
    {
        cbt_toggle = GetComponent<Toggle>();
        cbt_toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(cbt_toggle);
        }); 
        mainCamera = Camera.main;       // keep to wake up the camera! - otherwise null exception
        controlHelper = GameObject.Find("ControlHelper");        
    }

    // check if current scene is the probe builder
    void UpdateContainerIfNeeded(int colorProfile)
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "ProbeBuilder") {
            GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().SetColorScheme(colorProfile);
        }
    }
    
    void ToggleValueChanged(Toggle change) {
        controlHelper = GameObject.Find("ControlHelper");
        Debug.Log(">>>" + controlHelper);
        Debug.Log("Toggle Value Changed");
        if (change.isOn) {
            Debug.Log("Toggle is ON - ColorBlind Mode");
            try {
                controlHelper.GetComponent<ControlHelper>().ChangeColorProfile(2);
            } catch (System.Exception e) {
                // Debug.Log("Control Helper not found - debug mode only");
            }
            UpdateContainerIfNeeded(2);
        } else {
            Debug.Log("Toggle is OFF - Normal Mode");
            try {
                controlHelper.GetComponent<ControlHelper>().ChangeColorProfile(1);
            } catch (System.Exception e) {
                // Debug.Log("Control Helper not found - debug mode only");
            }
            UpdateContainerIfNeeded(1);
        }
    }
}
