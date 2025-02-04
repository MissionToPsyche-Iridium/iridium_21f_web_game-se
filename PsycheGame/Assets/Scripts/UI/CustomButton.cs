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

    v1.1 - Feb. 1 - refactored code by moving the color scheme to the TileColorScheme class (abstract)
    with the specific color schemes implementing the choice of colors.
*/
public class CustomButton : MonoBehaviour, IPointerDownHandler
{
    private AudioClip _swooshSound;

    private int currentProfile;

    private Volume volume;
    private ContainerManager containerManager;
    private TileColorScheme colorScheme;

    private void Awake()
    {
        //_swooshSound = Resources.Load<AudioClip>("Audio/laser-swoosh");
        //this.AddComponent<AudioSource>();
        containerManager = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>();
        currentProfile = containerManager.GetColorSchemeCode();

        volume = GameObject.Find("Box Volume").GetComponent<Volume>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // GetComponent<AudioSource>().PlayOneShot(_swooshSound, 1.0f);
        if (currentProfile == 1)
        {
            containerManager.SetColorScheme(2);
            currentProfile = 2;
        }
        else
        {
            containerManager.SetColorScheme(1);
            currentProfile = 1;
        }
        colorScheme = containerManager.GetColorScheme();
        volume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
        colorAdjustments.colorFilter.overrideState = true;
        colorAdjustments.postExposure.overrideState = true;
        colorAdjustments.postExposure.value = colorScheme.exposure;
        colorAdjustments.colorFilter.value = colorScheme.BaseSceneColor;
    }
}
