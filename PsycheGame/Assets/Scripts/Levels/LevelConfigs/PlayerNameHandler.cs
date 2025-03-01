using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        if (leaderBoard == null)
        {
            Debug.LogError("LeaderBoard script not found.");
            return;
        }

        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            playerNameObject.SetActive(false);
            validationMessage.gameObject.SetActive(false);
        }
        else
        {
            playerNameObject.SetActive(true);
            validationMessage.gameObject.SetActive(true);
            validationMessage.text = "Please enter a name.";
        }

        beginButton.onClick.AddListener(OnBeginButtonClicked);
    }

    private void OnBeginButtonClicked()
    {
        string playerName = playerNameField.text.Trim();
        Debug.Log("Player: " + playerName);
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            if (leaderBoard.IsPlayerNameUnique(playerName))
            {
                PlayerPrefs.SetString(PlayerNameKey, playerName);
                PlayerPrefs.Save();
                
                Debug.Log("Starting game for " + PlayerPrefs.GetString(PlayerNameKey));

                playerNameObject.SetActive(false);
                validationMessage.gameObject.SetActive(false);
                LevelManager.Instance.StartGame();
            }
            else
            {
                validationMessage.text = "Name already exists. Please choose another.";
            }
        }
        else
        {
            validationMessage.text = "Please enter a name.";
        }
    }
}