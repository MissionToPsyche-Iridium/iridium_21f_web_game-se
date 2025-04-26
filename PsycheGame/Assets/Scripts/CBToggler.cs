using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
    ColorBlind Mode Toggler :: CBToggler.cs
    Description: This script toggles the colorblind mode on and off, changing the game's color scheme based on the toggle state.

    Version History:
    - v1.1 (Feb 11): Updated logic to work with different scenes (e.g., main menu or builder scene).
    - v1.2 (Feb 17): Revised script to handle different scene use cases (splash/main and probe builder).
    - v1.3 (Apr 22): Optimize the script by removing redundant code and improving readability.
*/

public class CBToggler : MonoBehaviour
{
    [SerializeField] private Toggle cbtToggle;
    private ControlHelper controlHelper;
    private ContainerManager containerManager;
    private Scene currentScene;

    private const string ControlHelperName = "ControlHelper";
    private const string ContainerPanelName = "ContainerPanel";

    void Start()
    {
        cbtToggle = cbtToggle != null ? cbtToggle : GetComponent<Toggle>();
        controlHelper = GameObject.Find(ControlHelperName)?.GetComponent<ControlHelper>();
        containerManager = GameObject.Find(ContainerPanelName)?.GetComponent<ContainerManager>();
        currentScene = SceneManager.GetActiveScene();

        if (cbtToggle != null)
        {
            cbtToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        Debug.Log($"CBToggler: Active Scene is {currentScene.name}");
    }

    private void OnToggleValueChanged(bool isOn)
    {
        int colorProfile = isOn ? 2 : 1;

        if (controlHelper != null)
        {
            controlHelper.ChangeColorProfile(colorProfile);
        }
        else
        {
            Debug.LogWarning("ControlHelper not found. Defaulting to debug mode.");
        }

        UpdateContainerColorScheme(colorProfile);
    }

    private void UpdateContainerColorScheme(int colorProfile)
    {
        if (currentScene.name == "ProbeBuilder" && containerManager != null)
        {
            containerManager.SetColorScheme(colorProfile);
        }
        else if (containerManager == null)
        {
            Debug.LogWarning("ContainerManager not found. Unable to update color scheme.");
        }
    }
}
