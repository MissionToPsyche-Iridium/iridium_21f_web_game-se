using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/**
 * Snapshot.cs
 * 
 * This class creates a sprite from a 2D object.
 */

public class Snapshot
{
    private static int _layer = LayerMask.NameToLayer("Snapshot");

    private Canvas _target;

    public Snapshot(Canvas target)
    {
        _target = target;
    }

    public Sprite Take()
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 32);

        Camera originalCamera = _target.rootCanvas.worldCamera;

        Camera camera = GameObject.Instantiate(originalCamera).GetComponent<Camera>();
        camera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        camera.cullingMask = 1 << _layer;
        camera.targetTexture = renderTexture;

        camera.gameObject.name = "SnapshotCamera";
        camera.gameObject.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = false;

        int originalLayer = _target.gameObject.layer;
        _target.gameObject.layer = _layer;
        
        _target.rootCanvas.worldCamera = camera;

        camera.Render();

        Vector3[] worldCorners = new Vector3[4];
        _target.gameObject.GetComponent<RectTransform>().GetWorldCorners(worldCorners);

        Vector3[] corners = new Vector3[4];
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = camera.WorldToScreenPoint(worldCorners[i]);
        }

        int width = (int) (corners[2].x - corners[0].x),
            height = (int) (corners[2].y - corners[0].y);

        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        RenderTexture previousRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        texture.ReadPixels(new Rect(corners[0].x, corners[0].y, width, height), 0, 0);
        texture.Apply();

        RenderTexture.active = previousRenderTexture;

        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, width, height), new Vector2(0.5f, 0.5f));

        _target.gameObject.layer = originalLayer;

        _target.rootCanvas.worldCamera = originalCamera;

        GameObject.Destroy(camera.gameObject);
        Object.Destroy(renderTexture);

        return sprite;
    }
}
