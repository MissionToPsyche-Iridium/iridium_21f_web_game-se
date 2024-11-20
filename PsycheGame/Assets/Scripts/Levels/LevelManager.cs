
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject  missionObjectiveModalPanel;
    [SerializeField] GameObject pauseModalPanel;

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
        isPaused = true;
        UpdateButtonText(isPaused);
        Time.timeScale = 0f;
        missionObjectiveModalPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        missionObjectiveModalPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartGame()
    {
        pauseModalPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void UpdateButtonText(bool isPaused)
    {
        Transform textTransform = missionObjectiveModalPanel.transform.Find("BeginResumeText");

        if (textTransform != null)
        {
            TextMeshProUGUI textComponent = textTransform.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = isPaused ? "Resume" : "Begin";
            }
        }
        else
        {
            Debug.LogError("BeginResumeText object not found under the MissionObjectiveModalPanel.");
        }
    }
}