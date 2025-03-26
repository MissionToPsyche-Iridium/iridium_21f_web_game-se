using NUnit.Framework;
using UnityEngine;
using UnityEditor.SceneManagement;

[TestFixture]
public class ShipConfigLoadingTests
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
        Assert.NotNull(ShipManager.Ship); 
    }

}
