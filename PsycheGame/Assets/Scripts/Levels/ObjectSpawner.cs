using UnityEngine;

public class ObjectSpawner : MonoBehaviour {
    [SerializeField] protected Spawnable objectToSpawn;
    [SerializeField] protected GameObject boundingArea;
    [SerializeField] public int spawnInterval = 1;
    [SerializeField, Min(0)] public int objectLimit = 200;
    [SerializeField] public int initialPopulation = 0;

    [Header("Spawned Object Properties")]
    [SerializeField] public float scaleMin = 1f;
    [SerializeField] public float scaleMax = 5f;
    [SerializeField, Min(0f)] public float velocityMax = 20f;
    [SerializeField, Min(0f)] public float velocityMin = 1f;

    [Header("Spawnning Area")]
    [SerializeField] private bool showRadiusInEditor = true;
    [SerializeField, Min(1f)] private float spawnRadius = 50f;
    [SerializeField, Min(1f)] private float destoryRadius = 60f;

    public float DestroyRadius {get { return destoryRadius; }}
    
    private int objectCount = 0;
    protected Vector3 boundingAreaCenter;

    public void InitWithConfig(LevelConfig.ObjectSpawnerConfig config) {
        this.spawnInterval = config.spawnInterval;
        this.objectLimit = config.objectLimit;
        this.initialPopulation = config.initialPopulation;
        this.scaleMax = config.scaleMax;
        this.scaleMin = config.scaleMax;
        this.velocityMax = config.velocityMax;
        this.velocityMin = config.velocityMin;
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
                Vector3 pos = GetRandomPoisiton(false);
                Spawnable newObj = AddObject(pos);
                newObj.transform.Rotate(Vector3.forward * Random.Range(-45f, 45f));
                newObj.transform.localScale *= Random.Range(scaleMin, scaleMax);
            }
        }
    }

    private void InitialPopulation() {
        for (int i = 0; i < initialPopulation; i++) {
            Vector3 pos = GetRandomPoisiton(true);
            Spawnable newObj = AddObject(pos);
            newObj.transform.Rotate(Vector3.forward * Random.Range(0.0f, 360f));
        }
    }

    private Spawnable AddObject(Vector3 position) {
        GameObject newObject = Instantiate(
            objectToSpawn.gameObject,
            position,
            Quaternion.FromToRotation(Vector3.up, boundingAreaCenter - position),
            this.gameObject.transform
        );

        Spawnable spawnableScript = newObject.GetComponent<Spawnable>();
        spawnableScript.Spawner = this;
        spawnableScript.BoundingArea = this.boundingArea;
        spawnableScript.Velocity = Random.Range(velocityMin, velocityMax); 

        objectCount++;
        return spawnableScript;
    }

    protected Vector3 GetRandomPoisiton(bool withinCamera) {
        Vector3 pos = Random.insideUnitCircle;
        if (withinCamera) {
            return pos;
        }
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
