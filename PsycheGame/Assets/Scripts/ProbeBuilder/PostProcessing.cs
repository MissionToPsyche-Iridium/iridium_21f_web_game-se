using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
    Author: Shawn Chen  (adapted from Will Weissman and Unity Documentation)
    Version: 1.0  

    Description: This script is used to apply post processing effects to the camera.

*/

//[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostProcessing : MonoBehaviour
{
    // need shader and material to apply post processing effects
    public Shader postShader;
    Material postMaterial;
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // if no material declared, create a new material
        if (postMaterial == null)
        {
            postMaterial = new Material(postShader);
        }

        RenderTexture renderTexture = RenderTexture.GetTemporary(src.width, src.height, 0, src.format);
        
        Graphics.Blit(src, renderTexture, postMaterial);
        Graphics.Blit(renderTexture, dest);

        RenderTexture.ReleaseTemporary(renderTexture);
    }
}
