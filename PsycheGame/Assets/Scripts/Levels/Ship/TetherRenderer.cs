using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TetherRenderer : MonoBehaviour {
    [Header("General Refrences")]
    [SerializeField] private ShipTetherLogic logic;

    [Header("Settings")]
    [SerializeField] private int percision = 40;
    [SerializeField][Range(0, 20)] private float straightLineSpeed = 5;

    [Header("Animation")]
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField][Range(0.01f, 4)] private float startWaveSize = 2;

    [Header("Progression")]
    [SerializeField] private AnimationCurve progressionCurve;
    [SerializeField][Range(1, 50)] private float progressionSpeed = 1;

    private float waveSize = 0;
    private float moveTime = 0;
    private bool strightLine = true;
    private bool tethering = true;
    private LineRenderer lineRenderer;

    // should only be called from 'ShipTetherLogic.cs'
    public void initWithConfig(ShipConfig.TetherConfig config) {
        this.percision = config.resolution;
        this.straightLineSpeed = config.straightLineSpeed;
        this.startWaveSize = config.startWaveSize;
        this.progressionSpeed = config.progressionSpeed;
    }

    private void Awake() {
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.numCornerVertices = 1;
        lineRenderer.numCapVertices = 10;
        lineRenderer.positionCount = percision;
    }

    private void OnEnable() {
        moveTime = 0;
        waveSize = startWaveSize;
        strightLine = false;

        lineRenderer.positionCount = percision;
        for (int i = 0; i < percision; i++) {
            lineRenderer.SetPosition(i, logic.transform.position);
        }
        lineRenderer.enabled = true;
    }

    private void OnDisable() {
        lineRenderer.enabled = false;
        tethering = false;
    }

    private void Update() {
        moveTime += Time.deltaTime;
        DrawTether();
    }

    private void DrawTether() {
        if (!strightLine) {
            if (Mathf.Floor(lineRenderer.GetPosition(percision - 1).x) == Mathf.Floor(logic.tetherPoint.x)) {
                strightLine = true;
            } else {
                DrawTetherAnim();
            }
        } else {
            if (!isTething) {
                logic.Tether();
                tethering = true;
            }

            if (waveSize > 0) {
                waveSize -= Time.deltaTime * straightLineSpeed;
                DrawTetherAnim();
            } else {
                waveSize = 0;
                if (lineRenderer.positionCount != 2) {
                    lineRenderer.positionCount = 2;
                }
                DrawTetherLine();
            }
        }
    }

    private void DrawTetherLine() {
        lineRenderer.SetPosition(0, logic.transform.position);
        lineRenderer.SetPosition(1, logic.tetherPoint);
    }

    private void DrawTetherAnim() {
        for (int i = 0; i < percision; i++) {
            float delta = (float)i / ((float)percision - 1f);
            Vector2 offset = Vector2.Perpendicular(logic.tetherDistVec).normalized * animationCurve.Evaluate(delta) * waveSize;
            Vector2 targetPos = Vector2.Lerp(logic.transform.position, logic.tetherPoint, delta) + offset;
            Vector2 currentPos = Vector2.Lerp(logic.transform.position, targetPos, progressionCurve.Evaluate(moveTime) * progressionSpeed);
            lineRenderer.SetPosition(i, currentPos);
        }
    }

    public bool isTething { get => tethering; }
}
