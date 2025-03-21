using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayButton : MonoBehaviour, IPointerClickHandler
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
        SceneManager.LoadScene("ExplorationLevel");
        _audioSource.PlayOneShot(_swooshSound, 1.0f);
    }
}
