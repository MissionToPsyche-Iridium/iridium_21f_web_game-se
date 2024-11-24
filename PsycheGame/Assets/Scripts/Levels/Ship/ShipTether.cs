using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D))]
public class ShipTether : MonoBehaviour {
    [SerializeField] private Camera cameraRef;
    [SerializeField] private LayerMask tetherableLayers;
    [SerializeField] private float tetherLength;

    private Vector3 tetherPoint;
    private DistanceJoint2D distJoint;

    private void Start() {
        distJoint = this.GetComponent<DistanceJoint2D>(); 
        distJoint.enabled = false;
        distJoint.enableCollision = true;
        distJoint.autoConfigureDistance = false;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {

            RaycastHit2D hit = Physics2D.Raycast(
                origin:    cameraRef.ScreenToWorldPoint(Input.mousePosition),
                direction: Vector2.zero,
                distance:  Mathf.Infinity,
                layerMask: tetherableLayers
            );

            if (hit.collider != null) {
                tetherPoint = hit.point;
                tetherPoint.z = 0;
                distJoint.connectedAnchor = tetherPoint;
                distJoint.enabled = true;
                distJoint.distance = tetherLength;
            }

        }

        if (Input.GetMouseButtonUp(0)) {
            distJoint.enabled = false;
        }
    }

}
