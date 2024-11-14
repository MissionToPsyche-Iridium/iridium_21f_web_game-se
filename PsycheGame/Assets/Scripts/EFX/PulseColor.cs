using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseColor : MonoBehaviour
{
    public float fadeDuration = 3f;
    public Color color1 = Color.gray;
    public Color color2 = new Color(0.22f, 1f, 0.08f);

    private Color startColor;
    private Color endColor;
    private float lastColorChangeTime;

    Material material;

    void Start()
    {        
        material = gameObject.GetComponent<Image>().material;
        startColor = color1;
        endColor = color2;
    }

    void Update()
    {
        var ratio = (Time.time - lastColorChangeTime) / fadeDuration;
        ratio = Mathf.Clamp01(ratio);
        gameObject.GetComponent<Image>().color = Color.Lerp(startColor, endColor, ratio);
        // material.color = Color.Lerp(startColor, endColor, Mathf.Sqrt(ratio)); // A cool effect
        // material.color = Color.Lerp(startColor, endColor, ratio * ratio); // Another cool effect

        if (ratio == 1f)
        {
            lastColorChangeTime = Time.time;

            var temp = startColor;
            startColor = endColor;
            endColor = temp;
        }
    }
}
