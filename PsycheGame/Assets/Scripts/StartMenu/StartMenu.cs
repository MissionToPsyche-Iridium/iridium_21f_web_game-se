using UnityEngine;
using UnityEngine.SceneManagement; 

public class StartMenu : MonoBehaviour
{
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
}