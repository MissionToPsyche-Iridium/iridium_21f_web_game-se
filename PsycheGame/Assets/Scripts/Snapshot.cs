using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // TODO: Set camera position, convert texture to sprite, and dispose of any unnecessary objects
    public Texture Take()
    {
        int[] originalLayers = new int[_targets.Count];
        for (int i = 0; i < _targets.Count; i++)
        {
            originalLayers[i] = _targets[i].layer;
            _targets[i].layer = _layer;
        }

        RenderTexture renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);

        Camera tempCam = (new GameObject()).AddComponent<Camera>();
        tempCam.cullingMask = 1 << _layer;
        tempCam.targetTexture = renderTexture;

        renderTexture.Create();

        GameObject.Destroy(tempCam);

        for (int i = 0; i < _targets.Count; i++)
        {
            _targets[i].layer = originalLayers[i];
        }

        return renderTexture;
    }
}
