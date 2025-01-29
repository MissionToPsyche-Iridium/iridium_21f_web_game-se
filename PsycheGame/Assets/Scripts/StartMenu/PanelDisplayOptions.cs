using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PanelDisplayOptions : MonoBehaviour, IPointerDownHandler
{   
    public GameObject congratulationsText;
    public GameObject aboutText;

    public GameObject disclaimerText;

    public GameObject controls;

    public GameObject exitButton;
    public GameObject nextButton;
    private AudioClip _swooshSound;


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

    private void Awake()
    {
        _swooshSound = Resources.Load<AudioClip>("Audio/laser-swoosh");
        this.AddComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<AudioSource>().PlayOneShot(_swooshSound, 1.0f);
    }

}
