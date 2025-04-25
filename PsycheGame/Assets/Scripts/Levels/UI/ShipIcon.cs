using UnityEngine;

public class ShipIcon : MonoBehaviour
{
    private Transform ship;
    private RectTransform iconRectTransform;

    private void Awake()
    {
        iconRectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        ship = GameObject.Find("Ship").transform;
        if (ship == null) Debug.LogError("Ship GameObject not found in scene.");
        iconRectTransform.localPosition = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (ship == null) return;
        UpdateIconRotation();
    }

    private void UpdateIconRotation()
    {
        Vector3 forward = ship.up;
        float angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg - 90f;
        iconRectTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}