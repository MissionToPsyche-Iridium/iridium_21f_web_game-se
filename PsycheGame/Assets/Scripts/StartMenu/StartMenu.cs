using UnityEngine;
using UnityEngine.SceneManagement; 

public class StartMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Start Game!");
        SceneManager.LoadScene("ProbeBuilder");
    }
    
    public void LoadGame()
    {
        Debug.Log("LoadG Game");
        //Logic to load saved games
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
}