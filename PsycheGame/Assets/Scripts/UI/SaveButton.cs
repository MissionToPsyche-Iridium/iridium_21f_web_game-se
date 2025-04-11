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


        if (_containerManager.IsReadyToSave())
        {

            // Sprite probeSprite = (new Snapshot(GameObject.Find("/MasterCanvas/SpawnArea").GetComponent<Canvas>())).Take();

            InputPrompt prompt = Instantiate(_promptPrefab, GameObject.Find("/MasterCanvas").transform).GetComponent<InputPrompt>();
            prompt.SetPrompt("Enter text");
            prompt.SetCallback((input) =>
            {
                Debug.Log(input);
                bool saved = ContainerGameData.Instance.saveProbeDesign(input);
                if (saved) {
                    NotificationService.Notify("Successfully saved probe");
                } 
                else {
                    NotificationService.Notify("Cannot save more than 10 designs. Navigate to the browser to delete a design.");
                }
                
            });
           
        }
        else
        {
            NotificationService.Notify("Could not save probe due to grid abnormalities (component spacing or lack of parts). Please fix and try again.");
        }

        debounce = false;
    }

}
