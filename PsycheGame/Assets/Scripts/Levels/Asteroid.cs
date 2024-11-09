using UnityEngine;

public class Asteroid : Spawnable, ScannableObject {
    private bool scanned = false;

    public bool IsScanned => scanned;
    public string Description => "A space asteroid that would wipe out your probe on contact.";

    // For now just as a demo we print the instance id of the scanned asteroid
    // and then set 'IsScanned' to true telling the probe that this object no
    // longer needs to be scanned
    //
    // In the future we implement scanning specific logic to the asteroid here 
    public void Scan() {
        Debug.Log("Scanned Asteroid with id: " + this.GetInstanceID());
        scanned = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Boundary")) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
    }
}
