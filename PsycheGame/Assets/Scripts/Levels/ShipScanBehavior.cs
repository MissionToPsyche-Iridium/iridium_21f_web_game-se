using UnityEngine;

public class ShipScanBehavior : MonoBehaviour {
    [SerializeField] private bool debugDrawScanRaycasts = false;

    [SerializeField] private float distance = 10f;
    [SerializeField] private LayerMask scannableMask;

    [SerializeField] private float rayCount = 100.0f;
    [SerializeField] private float arcAngle = 180.0f;

    private RaycastHit2D hit;

    private void Update() {

        for (int i = 0; i < rayCount; i++) {
            var angleAxis = Quaternion.AngleAxis(arcAngle / (rayCount - 1) * i - arcAngle / 2, Vector3.forward);
            var direction = angleAxis * transform.up;

            Debug.Log(direction);

            hit = Physics2D.Raycast(
                transform.position, 
                direction, 
                distance, 
                scannableMask
            );

                Debug.DrawRay(
                    this.transform.position,
                    direction,
                    Color.red
                );
            if (hit) {

                Debug.DrawLine(
                    this.transform.position,
                    hit.point,
                    Color.red
                );
            }
        }


        /*
        hit = Physics2D.Raycast(
            this.transform.position, // where to shoot the raycast from
            this.transform.up,       // the direction to shoot the raycast
            distance,                // length of the raycast
            scannableMask            // mask of objects that this raycast can hit
        );


        if (hit) { 
            // hit was not 'null' meaning an object was hit
            if (debugDrawScanRaycasts) {
                Debug.DrawLine(
                    this.transform.position,
                    hit.point,
                    Color.red
                );
            }
        }
        */
    }
}
