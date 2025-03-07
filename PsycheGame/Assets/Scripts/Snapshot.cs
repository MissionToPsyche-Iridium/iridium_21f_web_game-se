using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Snapshot.cs
 * 
 * This class creates a sprite from a 2D object.
 */

public class Snapshot
{
    private static int _layer = LayerMask.NameToLayer("Snapshot");

    private RectTransform _target;

    public Snapshot(RectTransform target)
    {
        _target = target;
    }

    public Sprite Take()
    {
        int originalLayer = _target.gameObject.layer;
        _target.gameObject.layer = _layer;

        int originalMask = Camera.main.cullingMask;
        Camera.main.cullingMask = 1 << _layer;

        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        Camera.main.targetTexture = renderTexture;

        Camera.main.Render();

        Vector3[] worldCorners = new Vector3[4];
        _target.GetWorldCorners(worldCorners);

        Vector3[] corners = new Vector3[4];
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = Camera.main.WorldToScreenPoint(worldCorners[i]);
        }

        int width = (int) (corners[2].x - corners[0].x),
            height = (int) (corners[2].y - corners[0].y);

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        RenderTexture previousRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        texture.ReadPixels(new Rect(corners[0].x, corners[0].y, width, height), 0, 0);
        texture.Apply();

        RenderTexture.active = previousRenderTexture;

        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, width, height), new Vector2(0.5f, 0.5f));

        _target.gameObject.layer = originalLayer;

        Camera.main.cullingMask = originalMask;
        Camera.main.targetTexture = null;

        return sprite;
    }
}
