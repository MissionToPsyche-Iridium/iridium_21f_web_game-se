using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class CameraCapture : MonoBehaviour
{
    [SerializeField] private Image captureDisplayArea;
      private Texture2D screenCapture;

    // Start is called before the first frame update
    void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            StartCoroutine(CaptureCoroutine());
        }
    }

    IEnumerator CaptureCoroutine() {
        yield return new WaitForEndOfFrame();
        
        Rect regionToCapture = new Rect(0,0,Screen.width, Screen.height);
        screenCapture.ReadPixels(regionToCapture, 0,0, false);
        screenCapture.Apply();
        ShowCapture();

    }

    void ShowCapture() {
        Sprite sprite = Sprite.Create(screenCapture, new Rect(0.0f,0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        //captureDisplayArea.sprite = sprite;
    }
}
