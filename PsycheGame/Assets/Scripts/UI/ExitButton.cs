using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ExitButton : MonoBehaviour, IPointerDownHandler
{
    private AudioClip _swooshSound;

    private void Awake()
    {
        _swooshSound = Resources.Load<AudioClip>("Audio/laser-swoosh");
        this.AddComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SceneManager.LoadScene("MainMenu");
        GetComponent<AudioSource>().PlayOneShot(_swooshSound, 1.0f);
    }
}
