using UnityEngine;

public class DrillController : MonoBehaviour {
    [Header("Debug Options")]
    [SerializeField] private bool debugDrawLaserRaycasts = false;

    [Header("Laser Raycast Parameters")]
    [SerializeField] private float laserRange = 5f; 
    [SerializeField] private float laserArcAngle = 15f;     
    [SerializeField] private int rayCount = 10;
    [SerializeField] private LayerMask drillableMask = 6;

    [Header("Laser Animation")]
    [SerializeField] private GameObject laserEffect;

    private RaycastHit2D hit;
    private MineralCollection currentAsteroid;

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
            DrillAsteroid();
            Debug.Log($"Drill activated on: {currentAsteroid.name}");
        }
    }


    private void DeactivateLaser() {
        laserEffect.SetActive(false);
        currentAsteroid = null;
        Debug.Log("Laser deactivated.");
    }
    private void DrillAsteroid() {
        Debug.Log("Drilling initatied");
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * laserRange);
    }
}
