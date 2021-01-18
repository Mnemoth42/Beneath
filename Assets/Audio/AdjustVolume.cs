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
        
        
        float value = PlayerPrefs.GetFloat(parameter);
        Debug.Log($"{parameter}:{value}");
        mixer.SetFloat(parameter, value);
        if(slider)
        {
            slider.value = value;
        }
        mixer.GetFloat(parameter, out value);
        Debug.Log($"After Awake, {parameter} is {value}");
    }

    public void UpdateValue()
    {
        mixer.SetFloat(parameter, slider.value);
        PlayerPrefs.SetFloat(parameter, slider.value);
        Debug.Log($"Setting {parameter} to {slider.value}");
    }
}
