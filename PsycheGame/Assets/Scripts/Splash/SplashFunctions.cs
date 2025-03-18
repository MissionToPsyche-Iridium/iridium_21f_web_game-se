using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


public class SplashFunctions : MonoBehaviour, IPointerDownHandler
{
    private AudioClip _swooshSound;
    private AudioSource _audioSource;

     public void GoToMainMenuScene()
    {
        Debug.Log("Scene Change: Splash to MainMenu");
        SceneManager.LoadScene("MainMenu");
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
}
