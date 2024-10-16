using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildManager : MonoBehaviour
{
    public GameObject item1Prefab;   
    public GameObject item2Prefab;
    public GameObject spawnPoint;

    public void Start(){
        spawnPoint = GameObject.Find("SpawnArea");
    }

    public void SpawnItem1()
    {
        Debug.Log("Spawn Item 1");
        SpawnShape(item1Prefab);
    }

    public void SpawnItem2()
    {
        Debug.Log("Spawn Item 2");
        SpawnShape(item2Prefab);
    }

    void SpawnShape(GameObject shapePrefab)
    {
        // foreach (Transform child in shapeSpawnArea)
        // {
        //     Destroy(child.gameObject);
        // }

        GameObject shape = Instantiate(shapePrefab, spawnPoint.transform);
        shape.transform.localPosition = new Vector3(0, 0, 0); 
        shape.transform.localScale = Vector3.one;

        Debug.Log($"Spawned shape: {shape.name} at position {shape.transform.localPosition}");
    }
}