using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlsButton : MonoBehaviour, IPointerDownHandler
{
     private AudioClip _swooshSound;
    private AudioSource _audioSource;

    [SerializeField]
    public GameObject ControlsPanel;


    private void Awake()
    {
        _swooshSound = Resources.Load<AudioClip>("Audio/laser-swoosh");
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        _audioSource.PlayOneShot(_swooshSound, 1.0f);
        if (ControlsPanel.activeSelf)
        {
            ControlsPanel.SetActive(false);
        } else {
            ControlsPanel.SetActive(true);
        }
    }
}
