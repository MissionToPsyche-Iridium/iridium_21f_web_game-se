using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour 
{

    [SerializeField] private GameObject missionObjectiveModalPanel; 

    public static bool IsGamePaused { get; private set; } = true;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
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
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        missionObjectiveModalPanel.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;

        LevelManager.Instance.StartMissionTimer();
    }

    public void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0f; 
        missionObjectiveModalPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1f; 
        missionObjectiveModalPanel.SetActive(false);
    }

    public void RestartGame()
    {
        missionObjectiveModalPanel.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        IsGamePaused = false;
        LevelManager.Instance.RestartLevel();
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