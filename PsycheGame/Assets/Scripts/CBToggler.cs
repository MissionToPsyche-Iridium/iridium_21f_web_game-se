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

    version 1.2 (Feb 17)
    :: revised script to handle different scene use cases (splash/main and probe builder)
*/

public class CBToggler : MonoBehaviour
{
    public Toggle cbt_toggle;
    public GameObject controlHelper;
    Camera mainCamera;
    private Scene scene;

    void Start()
    {
        cbt_toggle = GetComponent<Toggle>();
        cbt_toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(cbt_toggle);
        }); 
        mainCamera = Camera.main;                   // keep to wake up the camera! - otherwise null exception
        controlHelper = GameObject.Find("ControlHelper");        

        scene = SceneManager.GetActiveScene();
        Debug.Log("CBT:: Active Scene is {" + scene.name + "}");
    }

    void UpdateContainerIfNeeded(int colorProfile)
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == "ProbeBuilder") {
            GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().SetColorScheme(colorProfile);
        }
    }
    
    void ToggleValueChanged(Toggle change) {
        try {
            controlHelper = GameObject.Find("ControlHelper");
            if (change.isOn) {
                controlHelper.GetComponent<ControlHelper>().ChangeColorProfile(2);
                UpdateContainerIfNeeded(2);
            } else {
                controlHelper.GetComponent<ControlHelper>().ChangeColorProfile(1);
                UpdateContainerIfNeeded(1);
            }
        } catch (System.Exception e) {
            Debug.Log("Control Helper not found - debug mode only - " + e.Message + " - suppressed");
            GameObject.Find("ContainerPanel").GetComponent<ContainerManager>().SetColorScheme(change.isOn ? 2 : 1);
        }
    }
}
