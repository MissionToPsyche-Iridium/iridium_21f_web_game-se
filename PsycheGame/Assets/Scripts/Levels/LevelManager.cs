
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject  missionObjectiveModalPanel;
    [SerializeField] GameObject pauseModalPanel;
    MissionState missionState;

    private bool isPaused = false;

   void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        missionObjectiveModalPanel.SetActive(false);
        Time.timeScale = 1f;
    }


    public void PauseGame()
    {
        pauseModalPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseModalPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartGame()
    {
        pauseModalPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}