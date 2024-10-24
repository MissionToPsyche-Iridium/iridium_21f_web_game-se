using UnityEngine;

// Any object that wishes to be spawned by the 'ObjectSpawner.cs' class should
// extend this class and add its self to the 'Object To Spawn' field in the
// unity editor. All spawned object specific behavior should be implemented in
// the subclass.
//
// The lifetime of a 'Spawnable' object is controlled by the object spawner it
// is attached to, all creation and destruction of objects is controled for you!
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
