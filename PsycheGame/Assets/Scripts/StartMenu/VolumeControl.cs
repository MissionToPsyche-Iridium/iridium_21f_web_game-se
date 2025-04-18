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
        AudioListener.volume = 0.5f;                    
        slider.value = AudioListener.volume;
    }

    public void UpdateVolume()
    {
        AudioListener.volume = slider.value;
    }
}
