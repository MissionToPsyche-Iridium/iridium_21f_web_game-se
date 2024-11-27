using System.Collections;
using UnityEngine;

public class LaserController : MonoBehaviour {
    [SerializeField] private GameObject laserEffect; // The laser animation GameObject
    [SerializeField] private float laserRange = 5f;  // The maximum range of the laser
    [SerializeField] private LayerMask asteroidLayer; // The layer of the target asteroids

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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, laserRange, asteroidLayer);
        if (hit.collider != null) {
            currentAsteroid = hit.collider.GetComponent<MineralCollection>();
            if (currentAsteroid != null) {
                Debug.Log($"Laser activated. Targeting: {currentAsteroid.name}");
            }
        }
    }

    private void DeactivateLaser() {
        laserEffect.SetActive(false);
        currentAsteroid = null;
        Debug.Log("Laser deactivated.");
    }

    private void DrillAsteroid() {
        if (currentAsteroid != null) {
            Debug.Log($"Drilling asteroid: {currentAsteroid.name}");
            // Enable drilling in the asteroid
            currentAsteroid.Scan(); // Optional: Trigger scanning if needed
        }
    }

    private void OnDrawGizmosSelected() {
        // Visualize the laser range in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * laserRange);
    }
}
