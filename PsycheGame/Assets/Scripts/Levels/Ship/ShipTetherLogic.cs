using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpringJoint2D))]
public class ShipTetherLogic : MonoBehaviour {
    [Header("General Refrences")]
    [SerializeField] private Tether tether;
    [SerializeField] private Camera mCamera;
    [SerializeField] private LayerMask tetherableLayers;

    [Header("Launching")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private float launchSpeed = 1.0f;

    [Header("No Launching")]
    [SerializeField] private bool autoConfigureDist = false;
    [SerializeField] private float targetDistance = 3.0f;
    [SerializeField] private float targetFrequency = 1.0f;

    private Rigidbody2D rb;
    private SpringJoint2D springJoint;
    private Vector2 tetherPoint;
    private Vector2 tetherDistVec;

    private void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        springJoint = this.GetComponent<SpringJoint2D>();

        tether.enabled = false;
        springJoint.enabled = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            SetTetherPoint();
            tether.enabled = true;
        }
        else if (Input.GetKey(KeyCode.Mouse0)) {

            if (launchToPoint && tether.isTething) {
                Vector2 targetPos = tetherPoint - (Vector2)this.transform.position;
                this.transform.position = Vector2.Lerp(this.transform.position, targetPos, Time.deltaTime * launchSpeed);
            } 

        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) {
            tether.enabled = false;
            springJoint.enabled = false;
        }
    }

    private void SetTetherPoint() {
        Vector3 pos = this.transform.position;
        Vector3 worldPoint = mCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 distanceVec = worldPoint - pos;

        RaycastHit2D hit = Physics2D.Raycast(
            origin:    pos,
            direction: distanceVec.normalized,
            distance:  Mathf.Infinity,
            layerMask: tetherableLayers
        );

        if (hit.collider != null) {
            tetherPoint = hit.point;
            tetherDistVec = tetherPoint - (Vector2)this.transform.position;
        }
    }

    public void Tether() {
        springJoint.autoConfigureDistance = false;

        if (!launchToPoint && !autoConfigureDist) {
            springJoint.distance = targetDistance;
            springJoint.frequency = targetFrequency;
        }

        if (!launchToPoint) {
            if (autoConfigureDist) {
                springJoint.autoConfigureDistance = true;
                springJoint.frequency = 0;
            }

            springJoint.connectedAnchor = tetherPoint;
            springJoint.enabled = true;
        } else {
            rb.velocity = Vector2.zero;
        }
    }
}
