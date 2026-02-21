using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void OnEnable()
    {
        AudioManager.Instance.SetSliderValue(_masterSlider, "Master");
        AudioManager.Instance.SetSliderValue(_musicSlider, "Music");
        AudioManager.Instance.SetSliderValue(_sfxSlider, "SFX");
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, "Master");
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, "Music");
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.Instance.SetVolume(value, "SFX");
    }

}
