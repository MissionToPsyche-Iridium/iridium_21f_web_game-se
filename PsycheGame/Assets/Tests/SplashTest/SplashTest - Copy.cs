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
public class SplashTest {
    private GameObject startButton;

     [SetUp]
     public void Setup() {
          EditorSceneManager.OpenScene("Assets/Scenes/Splash.unity", OpenSceneMode.Additive);
          startButton = GameObject.Find("StartButton");
     }

     [Test]
     public void TestButtonText() {
          Assert.That(startButton.GetComponent<TextMesh>().text, Is.EqualTo("Start"));
     }

     [Test]
     public void TestStartButton() {
          startButton.GetComponent<Button>().onClick.Invoke();
          string sceneName = SceneManager.GetActiveScene().name;
          Assert.That(sceneName, Is.EqualTo("MainMenu"));
     }


    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(startButton);
    }

}
