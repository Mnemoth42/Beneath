using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AdjustVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public string parameter = "MasterVolume";
    Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        
        mixer.GetFloat(parameter, out float value);
        value = PlayerPrefs.GetFloat(parameter, value);
        slider.value = value;
    }

    public void UpdateValue()
    {
        mixer.SetFloat(parameter, slider.value);
        PlayerPrefs.SetFloat(parameter, slider.value);
    }
}
