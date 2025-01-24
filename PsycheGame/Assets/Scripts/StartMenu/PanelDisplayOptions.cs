using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelDisplayOptions : MonoBehaviour
{   
    public GameObject congratulationsText;
    public GameObject aboutText;

    public GameObject disclaimerText;

    public GameObject controls;

    public GameObject exitButton;
    public GameObject nextButton;

    public void ViewAboutText() {
        ClearPanel();
        aboutText.SetActive(true);
        nextButton.SetActive(true);

    }

    public void ViewDisclaimerText(){
        ClearPanel();
        disclaimerText.SetActive(true);
        exitButton.SetActive(true);
        

    }

    public void ViewControls(){
        ClearPanel();
        controls.SetActive(true);
        exitButton.SetActive(true);

    }

    private void ClearPanel(){
        congratulationsText.SetActive(false);
        aboutText.SetActive(false);
        disclaimerText.SetActive(false);
        controls.SetActive(false);
        exitButton.SetActive(false);
        nextButton.SetActive(false);

    }

}
