using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CBToggler : MonoBehaviour
{
    // Start is called before the first frame update

    public Toggle cbt_toggle;
    public GameObject controlHelper;
    Camera mainCamera;

    void Start()
    {
        cbt_toggle = GetComponent<Toggle>();
        cbt_toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(cbt_toggle);
        }); 
        mainCamera = Camera.main;    // keep to wake up the camera! - otherwise null exception
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

    // Update is called once per frame
    void ToggleValueChanged(Toggle change) {
        controlHelper = GameObject.Find("ControlHelper");
        Debug.Log(">>>" + controlHelper);
        Debug.Log("Toggle Value Changed");
        if (change.isOn) {
            Debug.Log("Toggle is ON - ColorBlind Mode");
            controlHelper.GetComponent<ControlHelper>().ChangeColorProfile(2);
            UpdateContainerIfNeeded(2);
        } else {
            Debug.Log("Toggle is OFF - Normal Mode");
            controlHelper.GetComponent<ControlHelper>().ChangeColorProfile(1);
            UpdateContainerIfNeeded(1);
        }
    }
}
