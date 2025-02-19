using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ViewPanelToggle : MonoBehaviour, IPointerDownHandler
{
   public GameObject Panel;
   public GameObject HowToPlayButton;
   public GameObject ControlsButton;
   public GameObject InfoPanel;
   private AudioClip _swooshSound;

    private bool _previouslyActive = false;

   public void openPanel() {
        if(Panel != null) {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
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

    public void toggleControlsButton() {
        if(ControlsButton != null) {
            bool isActive = ControlsButton.activeSelf;
            ControlsButton.SetActive(!isActive);
        }
    }

    public void toggleInfoPanel() {
        if (InfoPanel.activeSelf)
        {
            _previouslyActive = true;
            InfoPanel.SetActive(false);
        }
        else if (_previouslyActive)
        {
            _previouslyActive = false;
            InfoPanel.SetActive(true);
        }
    }
}
