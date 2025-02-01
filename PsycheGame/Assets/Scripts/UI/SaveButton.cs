using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;

public class SaveButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private GameObject _containerManagerObject;
    [SerializeField]
    private GameObject _message;
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
        if (!debounce)
        {
            debounce = true;
        } else
        {
            return;
        }

        GetComponent<AudioSource>().PlayOneShot(_swooshSound, 1.0f);

        if (_containerManager.IsReadyToSave())
        {
            ContainerGameData.Instance.saveProbeDesign();
        }
        else
        {
            // _message.GetComponent<TextMeshProUGUI>().SetText("Could not save due to abnormal spacing");
            Debug.Log("Could not save due to abnormal spacing");
        }

        debounce = false;
    }
}
