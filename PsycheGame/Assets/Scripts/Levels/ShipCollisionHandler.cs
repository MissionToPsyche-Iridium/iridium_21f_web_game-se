using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipCollisionHandler : MonoBehaviour
{
    public GameObject modalPanel;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            DestroyShip();
        }
    }

    private void DestroyShip()
    {
        Destroy(gameObject);
        modalPanel.SetActive(true);
    }


}