using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayButton : MonoBehaviour, IPointerClickHandler
{
   private AudioClip _swooshSound;

    private void Awake()
    {
        _swooshSound = Resources.Load<AudioClip>("Audio/laser-swoosh");
        this.AddComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("ExplorationLevel");
        GetComponent<AudioSource>().PlayOneShot(_swooshSound, 1.0f);
    }
}
