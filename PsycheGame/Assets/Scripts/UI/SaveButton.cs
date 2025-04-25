using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;
using System;
using UnityEngine.UIElements;

public class SaveButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private GameObject _containerManagerObject;
    [SerializeField]
    private GameObject _notificationPrefab;
     [SerializeField]
    private GameObject _promptPrefab;
    private AudioClip _swooshSound;
    private ContainerManager _containerManager;
    private AudioSource _audioSource;

    private bool debounce;

    private void Awake()
    {
        _containerManager = _containerManagerObject.GetComponent<ContainerManager>();
        _swooshSound = Resources.Load<AudioClip>("Audio/laser-swoosh");
        debounce = false;
        _audioSource = gameObject.AddComponent<AudioSource>();

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (debounce)
        {
            return;
        }
        else
        {
            debounce = true;
        }

        _audioSource.PlayOneShot(_swooshSound, 1.0f);


        if (!_containerManager.IsEmpty())
        {

            // Sprite probeSprite = (new Snapshot(GameObject.Find("/MasterCanvas/SpawnArea").GetComponent<Canvas>())).Take();

            InputPromptService.Create("What would you like to name your probe?", (input) =>
            {
                if (ContainerGameData.Instance.saveProbeDesign(input)) {
                    NotificationService.Create("Successfully saved probe");
                } 
                else {
                    NotificationService.Create("Cannot save more than 10 designs. Navigate to the browser to delete a design.");
                }
            });
        }
        else
        {
            NotificationService.Create("No probe to save. Try dragging some components from the inventory on the left.");
        }

        debounce = false;
    }

}
