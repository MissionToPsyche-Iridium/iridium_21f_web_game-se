using UnityEngine;
using UnityEngine.UI;

public class ShipScanBehavior : MonoBehaviour {
    [Header("Debug Options")]
    [SerializeField] private bool debugDrawScanRaycasts = false;

    [Header("Scan Raycast Prameters")]
    [SerializeField] private float distance = 10f;    // length of rays
    [SerializeField] private float rayCount = 100.0f; // number of rays to fire
    [SerializeField] private float arcAngle = 180.0f; // angle to fire 'rayCount' rays across
    [SerializeField] private LayerMask scannableMask;

    [Header("Animation")]
    [SerializeField] private GameObject scanner;
    [SerializeField] private GameObject scanProgressBarPrefab;

    private RaycastHit2D hit;
    private bool isScanning = false;

    // UI element displaying popups for items that have been scanned
    // this is retrieved in 'Awake'
    private ScannedColumn scannedPopupColumn;

    private void Awake() {
        // TODO
        // there should be a better way to obtain an instance of the popup column
        // to allow this class to add entires this feels a bit hackish but it works
        // fine for now
        scannedPopupColumn = (ScannedColumn)FindObjectOfType(typeof(ScannedColumn));
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            isScanning = !isScanning;
            scanner.SetActive(isScanning);
        }

        if (isScanning) {
            ScanWithRaycasts();
        }
    }

    private void ScanWithRaycasts() {
        for (int i = 0; i < rayCount; i++) {
            var angleAxis = Quaternion.AngleAxis(arcAngle / (rayCount - 1) * i - arcAngle / 2, Vector3.forward);
            var direction = angleAxis * transform.up;

            hit = Physics2D.Raycast(
                transform.position, // location of raycast start point
                direction,          // direction to fire the raycast (computed above)
                distance,           // length of the raycast
                scannableMask       // mask of object types this raycast hits
            );

            // If [hit] is not null then we have hit a game object
            // in the 'scannable' layer
            if (hit) {
                if (debugDrawScanRaycasts) {
                    Debug.DrawLine(
                        this.transform.position,
                        hit.point,
                        Color.red
                    );
                }
                OnRaycastHit();
            }
        }
    }

    // called when a raycast hits an object in the 'scannable' layer
    private void OnRaycastHit() {
        GameObject scannedObj = hit.transform.gameObject;
        if (!scannedObj.TryGetComponent<ScannableObject>(out var scannable)) {
            Debug.LogError(
                "Scanned an object whose layer is set to 'scannable' but whose base " +
                "class was not derived from 'ScannableObject.cs'.\nEnsure all objects " +
                "in layer scannable extend the scannable class!\nScanned Object Name: " + 
                scannedObj.name
            );
            return;
        }

        if (!scannable.ScanProgress.isComplete) {
            if (scannable.ScanProgress.Value == 0) {
                float scale_y = scannable.GameObject.transform.localScale.y;
                Vector3 pos = scannable.GameObject.transform.position;
                pos.y += scale_y * 2;

                GameObject uiObj = Instantiate(scanProgressBarPrefab, 
                                               pos,
                                               Quaternion.identity, 
                                               scannable.GameObject.transform);
                uiObj.transform.localScale = Vector3.one;
                uiObj.GetComponentInChildren<ProgressBarUI>().scanning = scannable;
            }
            scannable.Scan();
        } else {
            var description = scannable.Description;
            var image = scannable.Image;
            var id = scannable.GetHashCode();
            scannedPopupColumn.AddEntry(image, "Item Scanned!", description, id);
        }
    }
}
