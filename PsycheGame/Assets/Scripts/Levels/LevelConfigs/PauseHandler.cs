using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour 
{

    [SerializeField] private GameObject missionObjectivePanel; 
    [SerializeField] private GameObject playerNameObject;
    [SerializeField] private Button beginButton; 
    [SerializeField] private TextMeshProUGUI validationMessage;
    private LeaderBoard leaderBoard;
    private TMP_InputField playerNameField;
    private string typedValue;
    public static bool IsGamePaused { get; private set; } = true;

    private const string PlayerNameKey = "PlayerName"; 

    private void Awake()
    {
        playerNameField = playerNameObject.GetComponent<TMP_InputField>();
        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            playerNameObject.SetActive(false);
            beginButton.interactable = true;
            validationMessage.gameObject.SetActive(false);
        }
        else
        {
            playerNameObject.SetActive(true);
            beginButton.interactable = false;
            validationMessage.gameObject.SetActive(true);
            validationMessage.text = "Please enter a name.";
        }

        playerNameField.onDeselect.AddListener(OnDeselectInputField);
        beginButton.onClick.AddListener(OnBeginButtonClicked);
    }

    public void OnDeselectInputField(string input)
    {
        string playerName = input.Trim();
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            if (leaderBoard.IsPlayerNameUnique(playerName))
            {
                validationMessage.text = "Name is valid.";
                beginButton.interactable = true;
            }
            else
            {
                validationMessage.text = "Name already exists. Please choose another.";
                beginButton.interactable = false;
            }
        }
        else
        {
            beginButton.interactable = false;
            validationMessage.text = "Please enter a name.";
        }
    }

    private void OnBeginButtonClicked()
    {
        string playerName = playerNameField.text.Trim();
        PlayerPrefs.SetString(PlayerNameKey, playerName);
        PlayerPrefs.Save();

        playerNameObject.SetActive(false);
        validationMessage.gameObject.SetActive(false);
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
}