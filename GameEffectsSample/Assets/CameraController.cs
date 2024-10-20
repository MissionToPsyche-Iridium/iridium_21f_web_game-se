using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour {

    private Camera _camera;
    private Vector2 viewPortSize;
    private Vector3 currentVelocity;
    private Vector3 targetPosition;
    private Vector2 distance;

    [SerializeField] private Transform toFollow;
    [SerializeField] private float viewPortFactor = 1.0f;
    [SerializeField] private float followDuration;
    [SerializeField] private float maximumFollowSpeed;

    void Start() {
        this._camera = this.GetComponent<Camera>();
        targetPosition = toFollow.position;
    }

    void Update() {
        viewPortSize = (_camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) - _camera.ScreenToWorldPoint(Vector2.zero)) * viewPortFactor;

        distance = toFollow.position - transform.position;
        if (Mathf.Abs(distance.x) > viewPortSize.x / 2) {
            targetPosition.x = toFollow.position.x - (viewPortSize.x / 2 * Mathf.Sign(distance.x));

        }

        if (Mathf.Abs(distance.y) > viewPortSize.y / 2) {
            targetPosition.y = toFollow.position.y - (viewPortSize.y / 2 * Mathf.Sign(distance.y));
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, followDuration, maximumFollowSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f); // lock the z component of the vector
    }

    private void OnDrawGizmos() {
        Color c = Color.red;
        c.a = 0.3f;
        Gizmos.color = c;
        Gizmos.DrawCube(transform.position, viewPortSize);
    }
}
