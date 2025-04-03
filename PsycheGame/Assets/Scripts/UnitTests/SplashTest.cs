using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor;

[TestFixture]
public class SplashTest
{
   Mouse cursor;

   public override void Setup() {
        base.Setup();
        SceneManager.LoadScene("Splash");
        cursor = InputSystem.AddDevice<Cursor>();

   }

   [UnityTest]
   public IEnumerator TestStartButton() {
        GameObject startButton = GameObject.Find("StartButton");
        ClickElement(startButton);
        yield return new WaitForSeconds(2f);

        string sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("MainMenu"));
   }

   public void ClickElement(GameObject element) {
    Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    Vector3 pos = camera.WorldToScreenPoint(element.transform.position);
    Set(cursor.position, pos);
    Click(cursor.leftButton)
   }

}
