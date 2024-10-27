using UnityEngine;

public abstract class Tooltip : MonoBehaviour {
    // pivots used in calculating tool tip pivot
    private static Vector2 pivotTopLeft = new Vector2(-0.05f, 1.05f);
    private static Vector2 pivotTopRight = new Vector2(1.05f, 1.05f);
    private static Vector2 pivotBottomLeft = new Vector2(-0.05f, -0.05f);
    private static Vector2 pivotBottomRight = new Vector2(1.05f, -0.05f);

    // calculate the pivot point based on where the tool tip is to be rendered
    // when rendering at the mouse position this has the effect of offseting
    // the tool tip to prevent it from being rendered off screen
    public virtual Vector2 CalculatePivot(Vector2 tipPosition) {
        Vector2 normalizedPos = new (tipPosition.x / Screen.width, tipPosition.y / Screen.height);

        if (normalizedPos.x < 0.5f && normalizedPos.y >= 0.5f) {
            return pivotTopLeft;
        }
        else if (normalizedPos.x > 0.5f && normalizedPos.y >= 0.5f) {
            return pivotTopRight;
        }
        else if (normalizedPos.x <= 0.5f && normalizedPos.y < 0.5f) {
            return pivotBottomLeft;
        }
        else {
            return pivotBottomRight;
        }
    }
}
