using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

[TestFixture]
public class SplashTest
{
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("SplashTest");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "SplashTest");
    }
    
    [TearDown]
    public void TearDown()
    {
        Debug.Log("TearDown completed");
    }

    [UnityTest]
    public IEnumerator Test_StartButton_SwooshSound()
    {
        var startButton = GameObject.Find("StartButton");
        Assert.IsNotNull(startButton, "StartButton not found");

        var button = startButton.GetComponent<Button>();
        Assert.IsNotNull(button, "Button component not found on StartButton");

        var text = startButton.GetComponentInChildren<TextMeshProUGUI>();
        Assert.IsNotNull(text, "TextMeshProUGUI component not found on StartButton");
        Assert.That(text.text, Is.EqualTo("Start"));

        var splashFunctions = startButton.GetComponent<SplashFunctions>();
        Assert.IsNotNull(splashFunctions, "SplashFunctions component not found");

        var audioSource = startButton.GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource, "AudioSource component not found");

        Debug.Log("Triggering OnPointerDown to simulate sound");
        splashFunctions.OnPointerDown(null);
        yield return null;

        Assert.IsTrue(audioSource.isPlaying, "Swoosh sound should be playing");

        Debug.Log("Clicking StartButton to trigger scene transition");
        button.onClick.Invoke();
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "MainMenu");
        yield return null;

        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Active scene: {sceneName}");
        Assert.That(sceneName, Is.EqualTo("MainMenu"));
    }
}