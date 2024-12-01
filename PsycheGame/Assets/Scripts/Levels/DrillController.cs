using UnityEngine;

public class DrillController : MonoBehaviour {
    [Header("Laser Raycast Parameters")]
    [SerializeField] private float laserRange = 5f; 
    [Header("Laser Animation")]
    [SerializeField] private GameObject laserEffect;

    private MineralCollection currentAsteroid;
    private const float drillDuration = 2f;
    private float drillTimer = 0f;

    private void Update() {
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

    private void ActivateLaser() {
        laserEffect.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        MineralCollection asteroid = other.GetComponent<MineralCollection>();
        Debug.Log(asteroid);
        if (asteroid != null) {
            currentAsteroid = asteroid;
            Debug.Log($"Drill activated on: {currentAsteroid.name}");
        }
    }


    private void DeactivateLaser() {
        laserEffect.SetActive(false);
        drillTimer = 0f;
        currentAsteroid = null;
        Debug.Log("Laser deactivated.");
    }
    private void DrillAsteroid() {
        drillTimer += Time.deltaTime;
        if (drillTimer >= drillDuration) {
            currentAsteroid.Drill();
            drillTimer = 0f; 
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * laserRange);
    }
}
