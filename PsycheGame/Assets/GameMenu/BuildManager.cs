using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildManager : MonoBehaviour
{
    public GameObject circlePrefab;   
    public GameObject squarePrefab;
    public GameObject spawnPoint;

    public void Start(){
        spawnPoint = GameObject.Find("SpawnArea");
    }

    public void SpawnCircle()
    {
        Debug.Log("Spawn Circle");
        SpawnShape(circlePrefab);
    }

    public void SpawnSquare()
    {
        Debug.Log("Spawn Square");
        SpawnShape(squarePrefab);
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