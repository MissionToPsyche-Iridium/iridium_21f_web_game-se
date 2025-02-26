using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour 
{

    [SerializeField] private GameObject missionObjectivePanel; 
    [SerializeField] private InputField playerNameField;
    [SerializeField] private Button beginButton; 
    public static bool IsGamePaused { get; private set; } = true;

    private const string PlayerNameKey = "PlayerName"; 

    private void Awake()
    {
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            nameInputField.gameObject.SetActive(false);
            beginButton.interactable = true;
            validationMessage.gameObject.SetActive(false);
        }
        else
        {
            nameInputField.gameObject.SetActive(true);
            beginButton.interactable = false;
            validationMessage.gameObject.SetActive(false);
        }

        nameInputField.onValueChanged.AddListener(OnNameInputChanged);
        beginButton.onClick.AddListener(OnBeginButtonClicked);
    }

    private void OnBeginButtonClicked()
    {
        string playerName = nameInputField.text.Trim();
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            PlayerPrefs.SetString(PlayerNameKey, playerName);
            PlayerPrefs.Save();

            nameInputField.gameObject.SetActive(false);
            validationMessage.gameObject.SetActive(false);

            StartGame();
        }
        else
        {
            validationMessage.gameObject.SetActive(true);
            validationMessage.text = "Please enter a valid name.";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");
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
        LevelManager.isLoading = false;
        Debug.Log("Level Manager is loading: " + LevelManager.isLoading);
        Debug.Log("Starting game for " + PlayerPrefs.GetString(PlayerNameKey));
        missionObjectivePanel.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
        LevelManager.Instance.StartMissionTimer();
    }

    public void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0f; 
        missionObjectivePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1f; 
        missionObjectivePanel.SetActive(false);
    }

    public void RestartGame()
    {
        missionObjectivePanel.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        IsGamePaused = true;
        LevelManager.Instance.RestartLevel();
    }

    public void UpdateButtonText(bool isPaused)
    {
        Transform textTransform = missionObjectivePanel.transform.Find("BeginResumeText");

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


    private void OnNameInputChanged(string input)
    {
        if (!string.IsNullOrWhiteSpace(input))
        {
            beginButton.interactable = true;
            validationMessage.gameObject.SetActive(false);
        }
        else
        {
            beginButton.interactable = false;
            validationMessage.gameObject.SetActive(true);
            validationMessage.text = "Please enter a name.";
        }
    }
}