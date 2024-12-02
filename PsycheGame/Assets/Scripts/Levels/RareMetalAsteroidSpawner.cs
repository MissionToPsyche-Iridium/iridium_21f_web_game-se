using System.Collections.Generic;
using UnityEngine;

public class RareMetalAsteroidSpawner : MonoBehaviour {
    [SerializeField] private GameObject rareMetalAsteroidPrefab;
    [SerializeField] private GameObject boundingArea;
    [SerializeField, Min(1)] private int rareAsteroidCount = 15;
    private Vector3 boundingAreaCenter;
    [SerializeField, Min(0.1f)] private float scaleMin = 1f;
    [SerializeField, Min(0.1f)] private float scaleMax = 5f;

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

    private void SpawnRareAsteroids()
    {
        for (int i = 0; i < rareAsteroidCount; i++)
        {
            Vector3 position = GetRandomPosition();
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
        Vector3 randomOffset = Random.insideUnitSphere * Random.Range(1f, 50f);
        randomOffset.z = 0;
        return boundingAreaCenter + randomOffset;
    }
}
