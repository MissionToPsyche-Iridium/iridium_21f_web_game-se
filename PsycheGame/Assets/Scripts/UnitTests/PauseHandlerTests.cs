using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEditor;
using System.Collections;
using TMPro;

[TestFixture]
public class PauseHandlerTests
{
    private GameObject testObject;
    private PauseHandler pauseHandler;
    private GameObject missionPanel;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject("TestPauseHandler");
        pauseHandler = testObject.AddComponent<PauseHandler>();

        missionPanel = new GameObject("MissionPanel");
        GameObject textObject = new GameObject("BeginResumeText");
        textObject.AddComponent<TextMeshProUGUI>();
        textObject.transform.SetParent(missionPanel.transform);

        var serializedObject = new SerializedObject(pauseHandler);
        serializedObject.FindProperty("missionObjectivePanel").objectReferenceValue = missionPanel;
        serializedObject.ApplyModifiedProperties();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testObject);
        Object.DestroyImmediate(missionPanel);
    }

    [Test]
    public void InitialState_IsNotPaused()
    {
        Assert.IsFalse(PauseHandler.IsGamePaused);
        Assert.AreEqual(1f, Time.timeScale);
    }

    [Test]
    public void PauseGame_SetsPausedStateAndTimeScale()
    {
        pauseHandler.PauseGame();

        Assert.IsTrue(PauseHandler.IsGamePaused);
        Assert.AreEqual(0f, Time.timeScale);
        Assert.IsTrue(missionPanel.activeSelf);
    }

    [Test]
    public void ResumeGame_ClearsPausedStateAndRestoresTimeScale()
    {
        pauseHandler.PauseGame();
        
        pauseHandler.ResumeGame();

        Assert.IsFalse(PauseHandler.IsGamePaused);
        Assert.AreEqual(1f, Time.timeScale);
        Assert.IsFalse(missionPanel.activeSelf);
    }

    [UnityTest]
    public IEnumerator UpdateButtonText_ChangesTextCorrectly()
    {
        pauseHandler.UpdateButtonText(true);
        TextMeshProUGUI textComponent = missionPanel.transform.Find("BeginResumeText").GetComponent<TextMeshProUGUI>();
        Assert.AreEqual("Resume", textComponent.text);

        pauseHandler.UpdateButtonText(false);
        yield return null; 
        Assert.AreEqual("Begin", textComponent.text);
    }

    [Test]
    public void UpdateButtonText_WithMissingTextComponent_LogsError()
    {
        Object.DestroyImmediate(missionPanel.transform.Find("BeginResumeText").gameObject);

        LogAssert.Expect(LogType.Error, "BeginResumeText object not found under the MissionObjectiveModalPanel.");
        pauseHandler.UpdateButtonText(true);
    }
}