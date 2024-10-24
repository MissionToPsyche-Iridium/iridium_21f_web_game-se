using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipCollisionHandler : MonoBehaviour
{
    [SerializeField] GameObject ship;
    [SerializeField] GameObject modalPanel;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("Ship hit by asteroid!");
            DestroyShip();

        }
    }

    private void DestroyShip()
    {
        modalPanel.SetActive(true);
        Destroy(ship);
    }
}