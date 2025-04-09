using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = AudioListener.volume;
    }

    public void UpdateVolume()
    {
        AudioListener.volume = slider.value;
    }
}
