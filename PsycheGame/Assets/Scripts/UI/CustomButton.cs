using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Runtime.CompilerServices;

/*
    CustomButton.cs

    Date: 11/27/2024
    Version 1.0

    Description: this script provides the behavior for the custom button.  Specifically, toggling
    between a preset profile of color settings.

    v 1.0 - Shawn - initial implementation of the custom button behavior with preset and handle to the Volume component.

*/
public class CustomButton : MonoBehaviour, IPointerDownHandler
{
    private class ColorProfile
    {
        public Color color;
        public float exposure;
    }

    private AudioClip _swooshSound;

    private ColorProfile[] colorProfiles = new ColorProfile[3]
    {
        new ColorProfile { color = new Color(1.0f, 1.0f, 1.0f, 1.0f), exposure = 0.0f },
        new ColorProfile { color = new Color(0.0f, 1.37f, 0f, 1.0f), exposure = -0.5f },
        new ColorProfile { color = new Color(0.95f, 0.87f, 1.47f, 1.0f), exposure = -0.5f }
    };
    private int toggleState;

    private Volume volume;

    private void Awake()
    {
        //_swooshSound = Resources.Load<AudioClip>("Audio/laser-swoosh");
        //this.AddComponent<AudioSource>();
        toggleState = 0;
        volume = GameObject.Find("Box Volume").GetComponent<Volume>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // GetComponent<AudioSource>().PlayOneShot(_swooshSound, 1.0f);

        if (toggleState == 2)
        {
            toggleState = 0;
        }
        else
        {
            toggleState++;
        }
        volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
        colorAdjustments.colorFilter.overrideState = true;
        colorAdjustments.postExposure.overrideState = true;
        colorAdjustments.postExposure.value = colorProfiles[toggleState].exposure;
        colorAdjustments.colorFilter.value = colorProfiles[toggleState].color;

        // Debug.Log("Color Adjustments: " + colorAdjustments.colorFilter.value);
    }
}
