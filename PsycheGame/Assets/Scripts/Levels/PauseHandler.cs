using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour 
{

    [SerializeField] private GameObject missionObjectiveModalPanel; 
    [SerializeField] private GameObject pauseModalPanel; 

    [SerializeField] private MissionTimer missionTimer;

    public static bool IsGamePaused { get; private set; } = true;


   void Update()
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
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        missionObjectiveModalPanel.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
        missionTimer.StartMissionTimer();
    }


    public void PauseGame()
    {
        IsGamePaused = true;
        UpdateButtonText(IsGamePaused);
        Time.timeScale = 0f;
        missionObjectiveModalPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        missionObjectiveModalPanel.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
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