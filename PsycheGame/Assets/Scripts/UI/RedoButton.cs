using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class RedoButton : MonoBehaviour, IPointerDownHandler
{
    private BuildManager _buildManager;
    private AudioClip _swooshSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        _buildManager = transform.parent.parent.gameObject.GetComponent<BuildManager>();
        _swooshSound = Resources.Load<AudioClip>("Audio/light-beam-hit-01");
        _audioSource = gameObject.AddComponent<AudioSource>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _audioSource.PlayOneShot(_swooshSound, 1.0f);
        _buildManager.Redo();
    }
}
