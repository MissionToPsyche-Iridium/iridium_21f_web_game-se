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
   public GameObject ControlsPanel;
   public GameObject InfoPanel;
   private AudioClip _swooshSound;
    private AudioSource _audioSource;
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
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _audioSource.PlayOneShot(_swooshSound, 1.0f);
    }

    public void toggleControls() {
        ControlsButton.SetActive(!ControlsButton.activeSelf);
        if (ControlsPanel.activeSelf)
        {
            ControlsPanel.SetActive(false);
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
