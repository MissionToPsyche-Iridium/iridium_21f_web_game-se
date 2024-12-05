using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//This script takes a screenshot of the entire screen when the save button is pressed.
//TODO: use the Unity camera in the scene to take the screenshot
//TODO: create a mask to only save the portion of the screen that shows the probe
//Reference tutorial: https://www.youtube.com/watch?v=d5nENoQN4Tw

public class Screenshot : MonoBehaviour
{
    private string fileName;
    public static void takeScreenshot(string filename) {
        ScreenCapture.CaptureScreenshot("Assets/Resources/Screenshots/"+filename+".png");
    }

    private IEnumerator ScreenShotCoroutine(string filename) {
        yield return new WaitForEndOfFrame();
         ScreenCapture.CaptureScreenshot("Assets/Resources/Screenshots/"+filename+".png");
        int width = 300;
        int height = 300;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(200,200,width,height);
        screenshotTexture.ReadPixels(rect,0,0);
        screenshotTexture.Apply();
        byte[] byteArray = screenshotTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes("Assets/Resources/Screenshots/"+filename+".png", byteArray);

    }
    

    
}

