using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
public class StartMenu : MonoBehaviour, IPointerDownHandler
{
    
    private AudioClip _swooshSound;
    private AudioSource _audioSource;

    public void GoToBuilderScene()
    {
        Debug.Log("Scene Change: MainMenu to ProbeBuilder");
        SceneManager.LoadScene("ProbeBuilder");
    }

    public void GoToFlyerScene() 
    { 
        Debug.Log("Scene Change: MainMenu to ProbeFlyer");
        SceneManager.LoadScene("ExplorationLevel");

    }
        public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
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