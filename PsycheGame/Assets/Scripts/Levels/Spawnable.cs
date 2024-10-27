using UnityEngine;

public abstract class Spawnable : MonoBehaviour {
    public GameObject BoundingArea { get; set; }
    public ObjectSpawner Spawner { get; set; }
    public float Velocity { get; set; }

    private void FixedUpdate() {
        this.transform.position += transform.up * (Time.deltaTime * Velocity);

        float distance = Vector3.Distance(transform.position, BoundingArea.transform.position);
        if (distance > Spawner.DestroyRadius) {
            Destroy(this.gameObject);
            Spawner.ChildDestroyed();
        }
    }
}
