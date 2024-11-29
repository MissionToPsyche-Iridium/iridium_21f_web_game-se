using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpringJoint2D))]
public class ShipTetherLogic : MonoBehaviour {
    [Header("General Refrences")]
    [SerializeField] private TetherRenderer tether;
    [SerializeField] private Camera mCamera;
    [SerializeField] private LayerMask tetherableLayers;

    [Header("Probe To Object")]
    [SerializeField] private float launchSpeed = 1.0f;
    [SerializeField] private float probeObjectDistance = 10f;

    private Rigidbody2D rb;
    private SpringJoint2D springJoint;
    [HideInInspector] public Vector2 tetherPoint;
    [HideInInspector] public Vector2 tetherDistVec;

    private void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        tether.enabled = false;

        springJoint = this.GetComponent<SpringJoint2D>();
        springJoint.enabled = false;
        springJoint.dampingRatio = 1.0f;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) {
            tether.enabled = SetTetherPoint();
        }
        else if(Input.GetKey(KeyCode.Mouse0) && tether.isTething) {
            // translate the ships position closer to the thering object
            // over time at the given launch speed
            transform.position = Vector2.Lerp(transform.position, tetherPoint, Time.deltaTime * launchSpeed);
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0)) {
            tether.enabled = false;
            springJoint.enabled = false;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1)) {
            tether.enabled = false;
            springJoint.enabled = false;
        }

        if (springJoint.connectedBody != null) {
            tetherPoint = springJoint.connectedBody.position; 
        }
    }

    private bool SetTetherPoint() {
        Vector3 pos = this.transform.position;
        Vector3 worldPoint = mCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 distanceVec = worldPoint - pos;

        RaycastHit2D hit = Physics2D.Raycast(
            origin:    pos,
            direction: distanceVec.normalized,
            distance:  distanceVec.magnitude,
            layerMask: tetherableLayers
        );

        if (hit.collider != null) {
            springJoint.connectedBody = hit.rigidbody;
            tetherPoint = hit.point;
            tetherDistVec = tetherPoint - (Vector2)this.transform.position;
            return true;
        }

        return false;
    }

    public void Tether() {
        springJoint.distance = probeObjectDistance;
        springJoint.enabled = true;
        rb.velocity = Vector2.zero;
    }
}
