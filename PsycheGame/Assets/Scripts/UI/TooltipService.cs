using log4net.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TooltipPivot
{
    Center,
    CenterLeft,
    CenterRight,
    Top,
    TopLeft,
    TopRight,
    Bottom,
    BottomLeft,
    BottomRight
}

public class TooltipService
{
    public static Tooltip Create(string title, string description, Vector3 position, TooltipPivot pivot = TooltipPivot.Center)
    {
        GameObject masterCanvas = GameObject.Find("/MasterCanvas");

        Tooltip tooltip = GameObject.Instantiate(Resources.Load<GameObject>("UI/TooltipCanvas"), masterCanvas.transform).GetComponent<Tooltip>();

        tooltip.SetTitle(title);
        tooltip.SetDescription(description);

        Rect rect = (tooltip.transform as RectTransform).rect;

        Vector3 pivotedPosition = position + Vector3.zero;
        switch (pivot)
        {
            case TooltipPivot.Center:
                break;
            case TooltipPivot.CenterLeft:
                pivotedPosition += new Vector3(rect.width / 2, 0.0f, 0.0f);
                break;
            case TooltipPivot.CenterRight:
                pivotedPosition += new Vector3(-rect.width / 2, 0.0f, 0.0f);
                break;
            case TooltipPivot.Top:
                pivotedPosition += new Vector3(0.0f, -rect.height / 2, 0.0f);
                break;
            case TooltipPivot.TopLeft:
                pivotedPosition += new Vector3(rect.width / 2, -rect.height / 2, 0.0f);
                break;
            case TooltipPivot.TopRight:
                pivotedPosition += new Vector3(-rect.width / 2, -rect.height / 2, 0.0f);
                break;
            case TooltipPivot.Bottom:
                pivotedPosition += new Vector3(0.0f, rect.height / 2, 0.0f);
                break;
            case TooltipPivot.BottomLeft:
                pivotedPosition += new Vector3(rect.width / 2, rect.height / 2, 0.0f);
                break;
            case TooltipPivot.BottomRight:
                pivotedPosition += new Vector3(-rect.width / 2, rect.height / 2, 0.0f);
                break;
        }

        Vector3[] worldCorners = new Vector3[4];
        (masterCanvas.transform as RectTransform).GetWorldCorners(worldCorners);

        tooltip.SetPosition(new Vector3(
            Mathf.Clamp(pivotedPosition.x, worldCorners[0].x + rect.width / 2, worldCorners[2].x - rect.width / 2),
            Mathf.Clamp(pivotedPosition.y, worldCorners[0].y + rect.height / 2, worldCorners[2].y - rect.height / 2),
            pivotedPosition.z
        ));

        return tooltip;
    }

    public static Tooltip Create(string title, string description, TooltipPivot pivot = TooltipPivot.Center)
    {
        Vector3 mousePos = Input.mousePosition;
        return Create(title, description, Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane)), pivot);
    }
}
