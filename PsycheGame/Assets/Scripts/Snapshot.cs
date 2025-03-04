using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Snapshot
{
    private const int DEFAULT_RESOLUTION_WIDTH = 250,
                      DEFAULT_RESOLUTION_HEIGHT = 250;

    private static int _layer = LayerMask.NameToLayer("Snapshot");

    private float _width, _height;
    private Vector3 _position;
    private List<GameObject> _targets;

    public Snapshot(float width, float height, Vector3 position, List<GameObject> targets)
    {
        _width = width;
        _height = height;
        _position = position;
        _targets = new List<GameObject>(targets);
    }

    public Sprite Take(int resolutionWidth, int resolutionHeight)
    {
        RenderTexture renderTexture = new RenderTexture(resolutionWidth, resolutionHeight, 24);

        Camera camera = GameObject.Instantiate(_targets[0].GetComponentInParent<Canvas>().rootCanvas.worldCamera) as Camera;
        camera.name = "SnapshotCamera";
        camera.cullingMask = 1 << _layer;
        camera.targetTexture = renderTexture;

        Canvas canvas = (new GameObject("SnapshotCanvas")).AddComponent<Canvas>();
        canvas.gameObject.layer = _layer;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        foreach (GameObject target in _targets)
        {
            GameObject targetClone = GameObject.Instantiate(target, canvas.transform);
            targetClone.layer = _layer;
            targetClone.name = "Target";
            targetClone.transform.position = target.transform.position;
        }

        canvas.worldCamera = camera;

        camera.Render();

        RenderTexture oldRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        Texture2D texture = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0.0f, 0.0f, resolutionWidth, resolutionHeight), 0, 0);
        texture.Apply();

        RenderTexture.active = oldRenderTexture;

        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, resolutionWidth, resolutionHeight), new Vector2(0.5f, 0.5f));

        GameObject.Destroy(camera.gameObject);
        GameObject.Destroy(canvas.gameObject);

        return sprite;
    }

    public Sprite Take()
    {
        return Take(DEFAULT_RESOLUTION_WIDTH, DEFAULT_RESOLUTION_HEIGHT);
    }
}
