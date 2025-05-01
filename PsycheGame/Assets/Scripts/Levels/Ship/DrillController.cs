using UnityEngine;

public class DrillController : MonoBehaviour {
    [Header("Laser Raycast Parameters")]
    [SerializeField] private float laserRange = 5f; 
    [Header("Laser Animation")]
    [SerializeField] public GameObject laserEffect;

    private MineralCollection currentAsteroid;
    private const float drillDuration = 2f;
    private float drillTimer = 0f;

    protected void Update() {
        HandleLaserActivation();
    }

    private void HandleLaserActivation() {
        if (Input.GetKeyDown(KeyCode.G)) {
            ActivateLaser();
        } else if (Input.GetKeyUp(KeyCode.G)) {
            DeactivateLaser();
        }

        if (laserEffect.activeSelf && currentAsteroid != null) {
            DrillAsteroid();
        }
    }

    public void ActivateLaser() {
        laserEffect.SetActive(true);
    }

    public void OnTriggerEnter2D(Collider2D other) {
        MineralCollection asteroid = other.GetComponent<MineralCollection>();
        if (asteroid != null) {
            currentAsteroid = asteroid;
            Debug.Log($"Asteroid detected: {asteroid.gameObject.name}");
        }
    }


    public void DeactivateLaser() {
        laserEffect.SetActive(false);
        drillTimer = 0f;
        currentAsteroid = null;
    }
    protected void DrillAsteroid() {
        drillTimer += Time.deltaTime;
        if (drillTimer >= drillDuration) {
            currentAsteroid.Drill();
            drillTimer = 0f; 
        }
    }

    protected void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * laserRange);
    }
}
