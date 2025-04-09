using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using TMPro;
using System.Collections;
using UnityEditor.SceneManagement;

[TestFixture]
public class PlayerNameHandlerTests
{
    private GameObject testObject;
    private PlayerNameHandler playerNameHandler;
    private GameObject playerNameObject;
    private InputField playerNameField;
    private Button beginButton;
    private TextMeshProUGUI validationMessage;
    private GameObject leaderBoardObject;

    [SetUp]
    public void Setup()
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

        leaderBoardObject = new GameObject("LeaderBoard");
        var leaderBoard = leaderBoardObject.AddComponent<LeaderBoard>();        
        leaderBoardObject.SetActive(false);
        typeof(LeaderBoard)
            .GetProperty("Instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            ?.SetValue(null, leaderBoard);
        leaderBoard.InitializeLeaderBoard();

        testObject = new GameObject("PlayerNameHandler");
        playerNameHandler = testObject.AddComponent<PlayerNameHandler>();
        
        playerNameObject = new GameObject("PlayerNameObject");
        playerNameField = playerNameObject.AddComponent<InputField>();
        
        GameObject buttonObject = new GameObject("BeginButton");
        beginButton = buttonObject.AddComponent<Button>();
        
        GameObject validationObject = new GameObject("ValidationMessage");
        validationMessage = validationObject.AddComponent<TextMeshProUGUI>();

        playerNameHandler.GetType()
            .GetField("playerNameObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(playerNameHandler, playerNameObject);
        playerNameHandler.GetType()
            .GetField("beginButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(playerNameHandler, beginButton);
        playerNameHandler.GetType()
            .GetField("validationMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(playerNameHandler, validationMessage);

        PlayerPrefs.DeleteAll();
    }

    [TearDown]
    public void TearDown()
    {
        if (testObject != null) Object.DestroyImmediate(testObject);
        if (playerNameObject != null) Object.DestroyImmediate(playerNameObject);
        if (beginButton != null) Object.DestroyImmediate(beginButton.gameObject);
        if (validationMessage != null) Object.DestroyImmediate(validationMessage.gameObject);
        if (leaderBoardObject != null) Object.DestroyImmediate(leaderBoardObject);
        PlayerPrefs.DeleteAll();
    }

    [UnityTest]
    public IEnumerator Test_Awake_ValidationMessageShownWhenNoName()
    {
        playerNameHandler.GetType()
            .GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(playerNameHandler, null);

        Assert.IsTrue(validationMessage.gameObject.activeSelf);
        Assert.AreEqual("Please enter a name.", validationMessage.text);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_Awake_ValidationMessageHiddenWhenNameExists()
    {
        PlayerPrefs.SetString("PlayerName", "TestPlayer");

        playerNameHandler.GetType()
            .GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(playerNameHandler, null);

        Assert.IsFalse(validationMessage.gameObject.activeSelf);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_OnBeginButtonClicked_ValidName_SavesAndHides()
    {
        PlayerPrefs.DeleteAll();
        playerNameHandler.GetType()
            .GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(playerNameHandler, null);
        playerNameField.text = "TestPlayer  ";
        
        LogAssert.Expect(LogType.Error, "LevelManager.Instance is null. Cannot start game.");

        playerNameHandler.GetType()
            .GetMethod("OnBeginButtonClicked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(playerNameHandler, null);

        Assert.AreEqual("TestPlayer", PlayerPrefs.GetString("PlayerName"));
        Assert.IsFalse(playerNameObject.activeSelf);
        Assert.IsFalse(validationMessage.gameObject.activeSelf);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_OnBeginButtonClicked_EmptyName_ShowsValidation()
    {
        PlayerPrefs.DeleteAll();
        playerNameHandler.GetType()
            .GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(playerNameHandler, null);
        playerNameField.text = "   "; 
        
        playerNameHandler.GetType()
            .GetMethod("OnBeginButtonClicked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(playerNameHandler, null);

        Assert.IsFalse(PlayerPrefs.HasKey("PlayerName"), "PlayerPrefs should not have PlayerName key for empty input");
        Assert.IsTrue(validationMessage.gameObject.activeSelf);
        Assert.AreEqual("Please enter a name.", validationMessage.text);
        yield return null;
    }

    [Test]
    public void Test_OnBeginButtonClicked_NullInputField_DoesNotCrash()
    {
        playerNameHandler.GetType()
            .GetMethod("Awake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(playerNameHandler, null);
        Object.DestroyImmediate(playerNameField);
        LogAssert.Expect(LogType.Error, "PlayerNameField is not assigned.");

        Assert.DoesNotThrow(() => {
            playerNameHandler.GetType()
                .GetMethod("OnBeginButtonClicked", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(playerNameHandler, null);
        });
        Assert.IsTrue(validationMessage.gameObject.activeSelf);
        Assert.AreEqual("Error: Name input field is missing.", validationMessage.text);
    }
}