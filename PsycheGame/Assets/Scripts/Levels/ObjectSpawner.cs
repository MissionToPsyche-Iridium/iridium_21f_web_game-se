using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {
    [System.Serializable]
    public record SpawnedObject {
        public Spawnable spawnable;
        public float scaleMin;
        public float scaleMax;
        public float velocityMin;
        public float velocityMax;
    }
    
    [System.Serializable]
    public record ObjectSpawnerConfig {
        [Tooltip("Batch size objects are spawned in when the current object count is less that the object limit")]
        public int spawnInterval;

        [Tooltip("Maximum number of objects that can be maintained by an object spawner at any single time")]
        public int objectLimit;

        [Tooltip("Number of objects that initially start inside of the spawning bounds on load (as opposed to spawning at the edge of bounds in 'Spawn Interval' batches)")]
        public int initialPopulation;

        [Tooltip("Object types which this spawner randomly selects from to spawn objects")]
        public List<SpawnedObject> objectsTypes;
    }

    [SerializeField] protected List<SpawnedObject> objectsToSpawn;
    [SerializeField] protected GameObject boundingArea;
    [SerializeField] public int spawnInterval = 1;
    [SerializeField, Min(0)] public int objectLimit = 200;
    [SerializeField] public int initialPopulation = 0;

    [Header("Spawnning Area")]
    [SerializeField] private bool showRadiusInEditor = true;
    [SerializeField, Min(1f)] private float spawnRadius = 50f;
    [SerializeField, Min(1f)] private float destoryRadius = 60f;

    public float DestroyRadius {get { return destoryRadius; }}
    
    private int objectCount = 0;
    protected Vector3 boundingAreaCenter;

    public void InitWithConfig(ObjectSpawnerConfig config) {
        this.spawnInterval = config.spawnInterval;
        this.objectLimit = config.objectLimit;
        this.initialPopulation = config.initialPopulation;
        this.objectsToSpawn = config.objectsTypes;
    }

    public void ChildDestroyed() {
        this.objectCount -= 1;
    }

    public void Start() {
        boundingAreaCenter = boundingArea.GetComponent<Renderer>().bounds.center;
        InitialPopulation();
    }

    private void Update() {
        MaintainPopulation(); 
    }

    private void MaintainPopulation() {
        if (objectCount < objectLimit) {
            for (int i = 0; i < spawnInterval; i++) {
                Vector3 pos = GetRandomPoisiton();
                Spawnable newObj = AddObject(pos);
                newObj.transform.Rotate(Vector3.forward * Random.Range(-45f, 45f));
            }
        }
    }

    private void InitialPopulation() {
        for (int i = 0; i < initialPopulation; i++) {
            Vector3 insideUnitCircle = Random.insideUnitCircle;
            Vector3 pos = boundingAreaCenter + insideUnitCircle * spawnRadius;
            Spawnable newObj = AddObject(pos);
            newObj.transform.Rotate(Vector3.forward * Random.Range(0.0f, 360f));
        }
    }

    private Spawnable AddObject(Vector3 position) {
        int randomIdx = Random.Range(0, objectsToSpawn.Count);
        SpawnedObject objectToSpawn = objectsToSpawn[randomIdx];

        GameObject newObject = Instantiate(
            objectToSpawn.spawnable.gameObject,
            position,
            Quaternion.FromToRotation(Vector3.up, boundingAreaCenter - position),
            this.gameObject.transform
        );

        Spawnable spawnableScript = newObject.GetComponent<Spawnable>();
        spawnableScript.Spawner = this;
        spawnableScript.BoundingArea = this.boundingArea;
        spawnableScript.Velocity = Random.Range(objectToSpawn.velocityMin, objectToSpawn.velocityMax);
        spawnableScript.transform.localScale *= Random.Range(objectToSpawn.scaleMin, objectToSpawn.scaleMax);

        objectCount++;
        return spawnableScript;
    }

    protected Vector3 GetRandomPoisiton() {
        Vector3 pos = Random.insideUnitCircle;
        pos = pos.normalized;
        pos *= spawnRadius;
        pos += boundingAreaCenter;
        return pos;
    }

    private void OnDrawGizmos() {
        if (showRadiusInEditor) {
            Color redColor = Color.red;
            redColor.a = 0.2f;
            Color greenColor = Color.green;
            greenColor.a = 0.2f;
            Vector3 center = boundingArea.GetComponent<Renderer>().bounds.center;
            Gizmos.color = redColor;
            Gizmos.DrawSphere(center, destoryRadius);
            Gizmos.color = greenColor;
            Gizmos.DrawSphere(center, spawnRadius);
        }
    }
}
