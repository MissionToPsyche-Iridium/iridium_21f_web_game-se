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

        foreach(SpriteRenderer spriteRenderer in spriteRendererArr) {
            spriteRenderer.color = new Color(slider.value, slider.value, slider.value, spriteRenderer.color.a);
        }
    }
}
