
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGameHandler : MonoBehaviour
{
    public GameObject modalPanel;
    public void RestartGame()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame(){
        SceneManager.LoadScene("MainMenu");
    }
}