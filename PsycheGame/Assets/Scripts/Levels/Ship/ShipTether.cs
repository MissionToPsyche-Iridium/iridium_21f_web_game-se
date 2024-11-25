using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(DistanceJoint2D))]
public class ShipTether : MonoBehaviour {
    [Header("General References")]
    [SerializeField] private Camera cameraRef;
    [SerializeField] private LayerMask tetherableLayers;
    [SerializeField] private float tetherLength;

    [Header("Tether Settings")]
    [SerializeField] private int percision = 40;
    [SerializeField][Range(0, 20)] private float straightenLineSpeed = 5f;

    [Header("Tether Animation")]
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField][Range(0.01f, 4)] private float startWaveSize = 2f;

    [Header("Tether Progression")]
    [SerializeField] private AnimationCurve progressionCurve;
    [SerializeField][Range(1, 50)] private float progressionSpeed = 1f;

    private Vector3 tetherPoint;
    private DistanceJoint2D distJoint;
    private LineRenderer lRenderer;

    private float waveSize = 0;
    private float moveTime = 0;
    private bool isTethering = true;
    private bool strightLine = false;


    private void Start() {
        waveSize = startWaveSize;

        lRenderer = this.GetComponent<LineRenderer>();
        lRenderer.enabled = false;
        lRenderer.positionCount = percision;

        distJoint = this.GetComponent<DistanceJoint2D>(); 
        distJoint.enabled = false;
        distJoint.enableCollision = true;
        distJoint.autoConfigureDistance = false;

    }

    private void Update() {
        moveTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(0)) {
            Vector3 origin = cameraRef.ScreenToWorldPoint(Input.mousePosition);
            Vector3 distanceVector = origin - this.transform.position;

            RaycastHit2D hit = Physics2D.Raycast(
                origin:    origin,
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

                DrawTether();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            lRenderer.enabled = false;
            distJoint.enabled = false;
            strightLine = false;
        }

        Debug.Log(waveSize);
    }

    private void DrawTether() {
        if (!strightLine) {
            if (lRenderer.GetPosition(percision - 1).x == tetherPoint.x) {
                strightLine = true;
            } else {
                DrawTetherAnim();
            }
        } else {
            if (waveSize > 0) {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawTetherAnim();
            } else {
                waveSize = 0;

                if (lRenderer.positionCount != 2) {
                    lRenderer.positionCount = 2;
                }

                DrawTetherLine();
            }
        }
    }

    // Draw the tether as a stright line
    private void DrawTetherLine() { 
        lRenderer.enabled = true;
        lRenderer.SetPosition(0, transform.position);
        lRenderer.SetPosition(1, tetherPoint);
    }

    // Draw the tether relative to its animaiton progress
    private void DrawTetherAnim() {
        lRenderer.enabled = true;
        Vector3 pos = this.transform.position;
        Vector3 distVec = tetherPoint - pos;        // distance between the two vectors

        for (int i = 0; i < percision; i++) {
            float delta = (float)i / ((float)percision - 1f);
            Vector2 offset = Vector2.Perpendicular(distVec).normalized * animationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPos = Vector2.Lerp(pos, tetherPoint, delta) + offset;
            Vector2 currentPos = Vector2.Lerp(pos, targetPos, progressionCurve.Evaluate(moveTime) * progressionSpeed);
            lRenderer.SetPosition(i, currentPos);
        }
    }
}
