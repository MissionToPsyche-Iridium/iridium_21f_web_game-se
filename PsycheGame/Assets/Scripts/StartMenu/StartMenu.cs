using UnityEngine;
using UnityEngine.SceneManagement; 

public class StartMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("StartGame!");
        SceneManager.LoadScene("ProbeBuilder");
    }
        public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}