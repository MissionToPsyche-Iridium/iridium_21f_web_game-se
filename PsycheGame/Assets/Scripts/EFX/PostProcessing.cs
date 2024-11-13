using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
    Author: Shawn Chen  (adapted from Will Weissman and Unity Documentation)
    Date: 11/13/2024
    Version: 1.0  

    Description: This script is used to apply post processing effects to the camera.

*/

public class PostProcessing : MonoBehaviour
{
    Camera BuildCamera;
    Camera TempCam;
    public Shader PostProcessingShader;
    public Shader Post_Outline;
    Material PostMaterial;


    void Start()
    {
        BuildCamera = GetComponent<Camera>();
        // BuildCamera.SetReplacementShader(PostProcessingShader, "RenderType");
        TempCam = new GameObject().AddComponent<Camera>();
        TempCam.enabled = false;
        PostMaterial = new Material(Post_Outline);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // temp cam setup
        TempCam.CopyFrom(BuildCamera);
        TempCam.clearFlags = CameraClearFlags.Color;
        TempCam.backgroundColor = Color.black;

        TempCam.cullingMask = 1 << LayerMask.NameToLayer("Outline");

        // create a new render texture
        RenderTexture TempRT = new RenderTexture(src.width, src.height, 0, RenderTextureFormat.R8);
        TempRT.Create();

        // set the temp cam's target texture
        TempCam.targetTexture = TempRT;

        // render all objects
        TempCam.RenderWithShader(PostProcessingShader, "");

        Graphics.Blit(TempRT, dest);

        TempRT.Release();
    }
}
