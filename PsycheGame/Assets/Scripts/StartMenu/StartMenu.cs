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
    public void GoToBuilderScene()
    {
        Debug.Log("Scene Change: MainMenu to ProbeBuilder");
        SceneManager.LoadScene("ProbeBuilder");
    }

    public void GoToFlyerScene() 
    { 
        Debug.Log("Scene Change: MainMenu to ProbeFlyer");
        //TODO SceneManager.LoadScene("");

    }
        public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    private void Awake()
    {
        _swooshSound = Resources.Load<AudioClip>("Audio/laser-swoosh");
        this.AddComponent<AudioSource>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<AudioSource>().PlayOneShot(_swooshSound, 1.0f);
    }
}