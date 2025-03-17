using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class NewDesignButton : MonoBehaviour, IPointerClickHandler
{
    private AudioClip _swooshSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        _swooshSound = Resources.Load<AudioClip>("Audio/laser-swoosh");
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("ProbeBuilder");
        _audioSource.PlayOneShot(_swooshSound, 1.0f);
    }
}
