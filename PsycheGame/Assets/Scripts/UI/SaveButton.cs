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

            Sprite probeSprite = (new Snapshot(GameObject.Find("/MasterCanvas/SpawnArea").GetComponent<Canvas>())).Take();

            Notification notification = Instantiate(_notificationPrefab, transform.parent.parent).GetComponent<Notification>();
            notification.SetImage(probeSprite);
            Boolean saved = ContainerGameData.Instance.saveProbeDesign();

            if(saved) {
                notification.SetMessage("Successfully saved probe");
            } 
            else {
                notification.SetMessage("Cannot save more than 10 designs. Navigate to the browser to delete a design.");
            }
        }
        else
        {
            bool exists = false;

            for (int i = 0; i < transform.parent.parent.childCount; i++)
            {
                GameObject child = transform.parent.parent.GetChild(i).gameObject;
                if (child.GetComponent<Notification>())
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                Instantiate(_notificationPrefab, transform.parent.parent).GetComponent<Notification>().SetMessage("Could not save probe due to grid abnormalities (component spacing or lack of parts). Please fix and try again.");
            }
        }

        debounce = false;
    }

}
