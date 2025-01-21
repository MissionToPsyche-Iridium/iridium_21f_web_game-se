using System.Collections.Generic;
using UnityEngine;

public class RareMetalAsteroidSpawner : MonoBehaviour {
    [SerializeField] private GameObject rareMetalAsteroidPrefab;
    [SerializeField] private GameObject boundingArea;
    [SerializeField, Min(1)] public int rareAsteroidCount = 15;
    private Vector3 boundingAreaCenter;
    [SerializeField, Min(0.1f)] public float scaleMin = 1f;
    [SerializeField, Min(0.1f)] public float scaleMax = 3f;

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

        Renderer renderer = asteroid.GetComponent<Renderer>();
    }

    private Vector3 GetRandomPosition()
    {
        float radius = boundingArea.GetComponent<Renderer>().bounds.extents.magnitude;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(0f, radius);
        Vector3 randomOffset = new Vector3(randomDirection.x, randomDirection.y, 0) * randomDistance;
        return boundingAreaCenter + randomOffset;
    }
}
