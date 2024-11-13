using UnityEngine;

public class Asteroid : Spawnable, ScannableObject {
    [SerializeField] private float progressIncr = 0.5f; // ammount of progress to gain each scan
    [SerializeField] private Sprite displayImage;

    private Progress scanProgress = new(Progress.NO_PROGRESS);

    public string Description => "A space asteroid that would wipe out your probe on contact.";
    public Sprite Image => displayImage;
    public Progress ScanProgress => scanProgress;

    // For now just as a demo we print the instance id of the scanned asteroid
    // and then set 'IsScanned' to true telling the probe that this object no
    // longer needs to be scanned
    //
    // In the future we implement scanning specific logic to the asteroid here 
    public void Scan() {
        scanProgress = scanProgress.incr(progressIncr);
        Debug.Log("[" + this.GetInstanceID() + "] Scanned Asteroid with progress: " + scanProgress.Value + "/100");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Boundary")) {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
        }
    }
}
