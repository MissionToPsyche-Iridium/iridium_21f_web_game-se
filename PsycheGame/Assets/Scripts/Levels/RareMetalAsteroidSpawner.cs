using System.Collections.Generic;
using UnityEngine;

public class RareMetalAsteroidSpawner : MonoBehaviour {
    [SerializeField] private GameObject rareMetalAsteroidPrefab;
    [SerializeField] private GameObject boundingArea;
    [SerializeField, Min(1)] public int rareAsteroidCount = 15;
    private Vector3 boundingAreaCenter;
    [SerializeField, Min(0.1f)] public float scaleMin = 1f;
    [SerializeField, Min(0.1f)] public float scaleMax = 5f;

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
        Vector3 randomOffset = Random.insideUnitSphere * Random.Range(1f, radius);
        randomOffset.z = 0;
        return boundingAreaCenter + randomOffset;
    }
}
