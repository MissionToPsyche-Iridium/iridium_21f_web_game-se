using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            //Need a pop up modal or text saying game over
            DestroyShip();
        }
    }

    private void DestroyShip()
    {
        Destroy(gameObject);
        Invoke("RestartGame", 1f);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}