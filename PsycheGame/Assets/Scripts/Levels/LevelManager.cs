
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
     [SerializeField] GameObject  missionObjectiveModalPanel;

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        missionObjectiveModalPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}