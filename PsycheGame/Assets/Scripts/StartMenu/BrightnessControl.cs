using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BrightnessControl : MonoBehaviour
{

    private SpriteRenderer[] spriteRendererArr;
    [SerializeField] Slider slider;

    private void Start()
    {
        spriteRendererArr = FindObjectsOfType<SpriteRenderer>();
    }

    public void SetBrightness() {

        // foreach(SpriteRenderer spriteRenderer in spriteRendererArr) {
        //     spriteRenderer.color = new Color(sliderValue, sliderValue, sliderValue, spriteRenderer.color.a);
        // }

        GameObject brightnessOverlay = GameObject.Find("Brightness");
        brightnessOverlay.GetComponent<Image>().color = new Color(brightnessOverlay.GetComponent<Image>().color.r, brightnessOverlay.GetComponent<Image>().color.g, brightnessOverlay.GetComponent<Image>().color.b, slider.value);
    }
}
