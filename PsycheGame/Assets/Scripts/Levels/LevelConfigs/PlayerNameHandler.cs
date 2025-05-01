using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameHandler : MonoBehaviour 
{

    [SerializeField] private GameObject playerNameObject;
    [SerializeField] private Button beginButton; 
    [SerializeField] private TextMeshProUGUI validationMessage;
    private LeaderBoard leaderBoard;
    private InputField playerNameField;

    private const string PlayerNameKey = "PlayerName"; 

    private void Awake()
    {
        playerNameField = playerNameObject.GetComponent<InputField>();
        leaderBoard = LeaderBoard.Instance;
        playerNameObject.SetActive(true);
        if (leaderBoard == null)
        {
            Debug.LogError("LeaderBoard script not found.");
            return;
        }

        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            validationMessage.gameObject.SetActive(false);
        }
        else
        {
            validationMessage.gameObject.SetActive(true);
            validationMessage.text = "Please enter a name.";
        }

        beginButton.onClick.AddListener(OnBeginButtonClicked);
    }

private void OnBeginButtonClicked()
{
    if (playerNameField == null)
    {
        Debug.LogError("PlayerNameField is not assigned.");
        validationMessage.gameObject.SetActive(true);
        validationMessage.text = "Error: Name input field is missing.";
        return;
    }

    string playerName = playerNameField.text?.Trim() ?? "";
    
    if (string.IsNullOrWhiteSpace(playerName))
    {
        validationMessage.gameObject.SetActive(true);
        validationMessage.text = "Please enter a name.";
        return;
    }

    PlayerPrefs.SetString(PlayerNameKey, playerName);
    PlayerPrefs.Save();
    
    Debug.Log("Starting game for " + PlayerPrefs.GetString(PlayerNameKey));

    playerNameObject.SetActive(false);
    validationMessage.text = ""; 
    validationMessage.gameObject.SetActive(false);

    if (LevelManager.Instance != null)
    {
        LevelManager.Instance.StartGame();
    }
    else
    {
        Debug.LogError("Instance is null. Cannot start game.");
    }
}
}