using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private Slider musicSlider, effectSlider;

    [SerializeField]
    private AudioMixer mixer;

    private void Start()
    {
        musicSlider.value = Game_Manager.instance.musicVolume;
        effectSlider.value = Game_Manager.instance.effectVolume;
    }

    public void MusicVolume()
    {
        float sliderValue = musicSlider.value;
        mixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
        Game_Manager.instance.musicVolume = sliderValue;
    }

    public void EffectsVolume()
    {
        float sliderValue = effectSlider.value;
        mixer.SetFloat("Effects", Mathf.Log10(sliderValue) * 20);
        Game_Manager.instance.effectVolume = sliderValue;
    }

}
