using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snapshot
{
    private int _layer;
    private List<GameObject> _targets;

    public Snapshot()
    {
        _layer = LayerMask.NameToLayer("Snapshot");
        _targets = new List<GameObject>();
    }

    public List<GameObject> GetTargets()
    {
        return new List<GameObject>(_targets);
    }

    public void AddTarget(GameObject target)
    {
        _targets.Add(target);
    }

    public void AddTargets(List<GameObject> targets)
    {
        foreach (GameObject target in targets)
        {
            AddTarget(target);
        }
    }

    public Sprite Take()
    {
        int[] originalLayers = new int[_targets.Count];
        Vector3 centerPosition = Vector3.zero;

        for (int i = 0; i < _targets.Count; i++)
        {
            originalLayers[i] = _targets[i].layer;
            _targets[i].layer = _layer;

            centerPosition += _targets[i].transform.position;
        }

        centerPosition /= _targets.Count;

        RenderTexture renderTexture = new RenderTexture(256, 256, 24);

        Camera tempCam = (new GameObject()).AddComponent<Camera>();
        tempCam.transform.position = centerPosition;
        tempCam.cullingMask = 1 << _layer;
        tempCam.targetTexture = renderTexture;

        Canvas rootCanvas = _targets[0].transform.root.GetComponent<Canvas>();

        Camera originalCamera = rootCanvas.worldCamera;
        rootCanvas.worldCamera = tempCam;

        tempCam.Render();

        rootCanvas.worldCamera = originalCamera;

        RenderTexture tempRender = RenderTexture.active;
        RenderTexture.active = renderTexture;

        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0.0f, 0.0f, texture.width, texture.height), 0, 0);
        texture.Apply();

        RenderTexture.active = tempRender;

        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        GameObject.Destroy(tempCam.gameObject);

        return sprite;
    }
}
