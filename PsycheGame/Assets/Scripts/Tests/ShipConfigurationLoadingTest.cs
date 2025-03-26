using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine.TestTools;

public class ShipConfigurationLoadingTest 
{
    private string scene_path = "Assets/Scenes/ExplorationLevel.unity";

    [SetUp]
    public void Setup() {
        EditorSceneManager.LoadScene(scene_path);
    }

    [TearDown]
    public void TearDown()
    {
        EditorSceneManager.UnloadSceneAsync(scene_path);
    }

    [Test]
    public void ShipManager_finds_ship_obj()
    {
    }

    [UnityTest]
    public IEnumerator PlayModeTestWithEnumeratorPasses()
    {
        Assert.NotNull(ShipManager.Ship);
        yield return null;
    }
}
