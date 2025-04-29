using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

[TestFixture]
public class MainMenuTest
{
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("MainMenuTest");
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "MainMenuTest");

        var popUpPanel = GameObject.Find("PopUpPanel");
        Assert.IsNotNull(popUpPanel, "PopUpPanel not found");
        Assert.IsTrue(popUpPanel.activeSelf, "PopUpPanel should be active on scene load");

        var modalExitButton = GameObject.Find("ExitButton");
        Assert.IsNotNull(modalExitButton, "Modal ExitButton not found");

        var button = modalExitButton.GetComponent<Button>();
        Assert.IsNotNull(button, "Button component not found on modal ExitButton");

        button.onClick.Invoke();
        yield return null;

        Assert.IsFalse(popUpPanel.activeSelf, "PopUpPanel should be inactive after clicking ExitButton");
    }

    [TearDown]
    public void TearDown()
    {
        Debug.Log("TearDown completed");
    }

    [UnityTest]
    public IEnumerator Test_AboutButton()
    {
        var aboutButton = GameObject.Find("AboutButton");
        Assert.IsNotNull(aboutButton, "AboutButton not found");

        var button = aboutButton.GetComponent<Button>();
        Assert.IsNotNull(button, "Button component not found on AboutButton");

        var text = aboutButton.GetComponentInChildren<TextMeshProUGUI>();
        Assert.IsNotNull(text, "TextMeshProUGUI component not found on AboutButton");
        Assert.That(text.text, Is.EqualTo("About"));

        button.onClick.Invoke();
        yield return null;

        var popUpPanel = GameObject.Find("PopUpPanel");
        Assert.IsNotNull(popUpPanel, "PopUpPanel not found");
        Assert.IsTrue(popUpPanel.activeSelf, "PopUpPanel should be active after AboutButton click");

        var aboutText = GameObject.Find("AboutText");
        Assert.IsNotNull(aboutText, "AboutText not found");
        Assert.IsTrue(aboutText.activeSelf, "AboutText should be active");

        var nextButton = GameObject.Find("NextButton");
        Assert.IsNotNull(nextButton, "NextButton not found");
        Assert.IsTrue(nextButton.activeSelf, "NextButton should be active");

        nextButton.GetComponent<Button>().onClick.Invoke();
        yield return null;

        var disclaimerText = GameObject.Find("DisclaimerText");
        Assert.IsNotNull(disclaimerText, "DisclaimerText not found");
        Assert.IsTrue(disclaimerText.activeSelf, "DisclaimerText should be active");
    }

    [UnityTest]
    public IEnumerator Test_ControlPanel_Volume_Contrast()
    {
        var controlsButton = GameObject.Find("ControlsButton");
        Assert.IsNotNull(controlsButton, "ControlsButton not found");

        var button = controlsButton.GetComponent<Button>();
        Assert.IsNotNull(button, "Button component not found on ControlsButton");

        var text = controlsButton.GetComponentInChildren<TextMeshProUGUI>();
        Assert.That(text.text, Is.EqualTo("Controls"));

        button.onClick.Invoke();
        yield return null;

        var volumeSlider = GameObject.Find("VolumeSlider");
        Assert.IsNotNull(volumeSlider, "VolumeSlider not found");

        var volumeControl = volumeSlider.GetComponent<VolumeControl>();
        Assert.IsNotNull(volumeControl, "VolumeControl component not found");

        float volumeValue = 0.3f;
        volumeSlider.GetComponent<Slider>().value = 0.3f;
        volumeControl.UpdateVolume();
        yield return null;

        Assert.That(AudioListener.volume, Is.EqualTo(volumeValue).Within(0.01f));


        var brightnessSlider = GameObject.Find("BrightnessSlider");
        Assert.IsNotNull(brightnessSlider, "BrightnessSlider not found");

        var brightnessControl = brightnessSlider.GetComponent<BrightnessControl>();
        Assert.IsNotNull(brightnessControl, "BrightnessControl component not found");

        var brightnessOverlay = GameObject.Find("Brightness");
        Assert.IsNotNull(brightnessOverlay, "Brightness overlay not found");

        float brigthnessValue = 0.5f;
        brightnessSlider.GetComponent<Slider>().value = brigthnessValue;
        brightnessControl.SetBrightness();
        yield return null;

        var image = brightnessOverlay.GetComponent<Image>();
        Assert.IsNotNull(image, "Image component not found on Brightness overlay");
        Assert.That(image.color.a, Is.EqualTo(brigthnessValue).Within(0.01f));


        var exitButton = GameObject.Find("ExitButton");
        Assert.IsNotNull(exitButton, "ExitButton not found");
    }

    [UnityTest]
    public IEnumerator Test_BuildAProbeButton()
    {
        var playButton = GameObject.Find("PlayButton");
        Assert.IsNotNull(playButton, "PlayButton not found");

        var button = playButton.GetComponent<Button>();
        Assert.IsNotNull(button, "Button component not found on PlayButton");

        var text = playButton.GetComponentInChildren<TextMeshProUGUI>();
        Assert.IsNotNull(text, "TextMeshProUGUI component not found on PlayButton");
        Assert.That(text.text, Is.EqualTo("Build a Probe"));

        button.onClick.Invoke();
        yield return new WaitForSeconds(0.5f);

        string sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("ProbeBuilder"));
    }

    [UnityTest]
    public IEnumerator Test_FlyAProbeButton()
    {
        var flyButton = GameObject.Find("PlayButton (1)");
        Assert.IsNotNull(flyButton, "PlayButton (1) not found");

        var button = flyButton.GetComponent<Button>();
        Assert.IsNotNull(button, "Button component not found on PlayButton (1)");

        var text = flyButton.GetComponentInChildren<TextMeshProUGUI>();
        Assert.IsNotNull(text, "TextMeshProUGUI component not found on PlayButton (1)");
        Assert.That(text.text, Is.EqualTo("Fly a Probe"));
        
        button.onClick.Invoke();
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "ExplorationLevel");
        yield return null;
        string sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("ExplorationLevel"));
    }
}