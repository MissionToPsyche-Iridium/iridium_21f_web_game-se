using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using UnityEditor.SceneManagement;


[TestFixture]
public class MainMenuTest {
    private GameObject testButton;

     [SetUp]
     public void Setup() {
          EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity", OpenSceneMode.Additive);
          testButton = GameObject.Find("ExitButton");
     }

     [Test]
     public void TestExitButton() {
        Assert.That(testButton.GetComponent<TextMesh>().text, Is.EqualTo("Exit"));
        testButton.GetComponent<Button>().onClick.Invoke();
        bool popUpPanelActive = GameObject.Find("PopUpPanel").activeSelf;
        Assert.IsFalse(popUpPanelActive);
     }
     
     [Test]
    public void TestAboutButton() {
        testButton = GameObject.Find("AboutButton");
        Assert.That(testButton.GetComponent<TextMesh>().text, Is.EqualTo("About"));
        testButton.GetComponent<Button>().onClick.Invoke();
        bool popUpPanelActive = GameObject.Find("PopUpPanel").activeSelf;
        Assert.IsFalse(popUpPanelActive);
        testButton = GameObject.Find("NextButton");
        testButton.GetComponent<Button>().onClick.Invoke();
        testButton = GameObject.Find("ExitButton");
        testButton.GetComponent<Button>().onClick.Invoke();
        testButton = GameObject.Find("NextButton");
        testButton.GetComponent<Button>().onClick.Invoke();
    
    }

    [Test]
    public void TestControlsButton() {

    }

    [Test]
     public void TestBuildAProbeButton() {
        testButton = GameObject.Find("PlayButton");
        Assert.That(testButton.GetComponent<TextMesh>().text, Is.EqualTo("PlayButton"));
        testButton.GetComponent<Button>().onClick.Invoke();
        string sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("ProbeBuilder"));

     }

    [Test]
    public void TestFlyAProbeButton() {
    testButton = GameObject.Find("PlayButton (1)");
    Assert.That(testButton.GetComponent<TextMesh>().text, Is.EqualTo("PlayButton (1)"));
    testButton.GetComponent<Button>().onClick.Invoke();
    string sceneName = SceneManager.GetActiveScene().name;
    Assert.That(sceneName, Is.EqualTo("ExplorationLevel"));
    }


    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testButton);
    }

}
