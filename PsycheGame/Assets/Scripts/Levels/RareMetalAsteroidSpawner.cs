using System.Collections.Generic;
using UnityEngine;

public class RareMetalAsteroidSpawner : MonoBehaviour {
    [SerializeField] private GameObject rareMetalAsteroidPrefab;
    [SerializeField] private GameObject boundingArea;
    [SerializeField, Min(1)] public int rareAsteroidCount = 15;
    private Vector3 boundingAreaCenter;
    [SerializeField, Min(0.1f)] public float scaleMin = 0.5f;
    [SerializeField, Min(0.1f)] public float scaleMax = 1.5f;

    private void Start()
    {
        if (boundingArea == null)
        {
            Debug.LogError("Bounding area not assigned.");
            return;
        }

        boundingAreaCenter = boundingArea.GetComponent<Renderer>().bounds.center;
        SpawnRareAsteroids();
    }

    public void SpawnRareAsteroids()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < rareAsteroidCount; i++)
        {
            Vector3 position = GetRandomPosition();
            Debug.Log("Spawning asteroid at position: " + position);
            AddRareMetalAsteroid(position);
        }
    }

    private void AddRareMetalAsteroid(Vector3 position)
    {
        GameObject asteroid = Instantiate(
            rareMetalAsteroidPrefab,
            position,
            Quaternion.identity,
            this.transform
        );

        asteroid.transform.localScale *= Random.Range(scaleMin, scaleMax);
    }

    private Vector3 GetRandomPosition()
    {
        Bounds bounds = boundingArea.GetComponent<Renderer>().bounds;
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float fixedZ = 0;
        return new Vector3(randomX, randomY, fixedZ);

    }
}
