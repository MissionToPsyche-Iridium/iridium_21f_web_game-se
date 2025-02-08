using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;
using System;

public class SaveButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private GameObject _containerManagerObject;
    [SerializeField]
    private GameObject _notificationPrefab;
    private AudioClip _swooshSound;
    private ContainerManager _containerManager;
    private bool debounce;

    private void Awake()
    {
        _containerManager = _containerManagerObject.GetComponent<ContainerManager>();
        _swooshSound = Resources.Load<AudioClip>("Audio/laser-swoosh");
        debounce = false;
        this.AddComponent<AudioSource>();

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

        GetComponent<AudioSource>().PlayOneShot(_swooshSound, 1.0f);

        if (_containerManager.IsReadyToSave())
        {
            ContainerGameData.Instance.saveProbeDesign();
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
                Instantiate(_notificationPrefab, transform.parent.parent).GetComponent<Notification>().SetMessage("Could not save probe due to abnormal spacing. Please fix and try again.");
            }
        }

        debounce = false;
    }
}
