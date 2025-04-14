using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    private Transform shipTransform;
    private void Start()
    {
        shipTransform = transform.parent;    
    }

    private void LateUpdate()
    {
        Vector3 forward = shipTransform.up;
        float angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}