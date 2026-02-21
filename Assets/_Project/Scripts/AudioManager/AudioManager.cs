using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public AudioMixer _mixer;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public static AudioManager Instance
    {
        get
        {
            // Se non esiste già un'istanza, cerca nel gioco
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();

                // Se non è stato trovato, crea un nuovo oggetto AudioManager
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("AudioManager");
                    _instance = singletonObject.AddComponent<AudioManager>();
                    singletonObject.tag = "AudioManager"; // Assegna il tag "AudioManager"
                }
            }

            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetSliderValue(Slider slider, string group)
    {
        if (_mixer.GetFloat(group, out float decibel))
        {
            float percentage = Mathf.Pow(10, decibel / 20);
            slider.value = percentage;
        }
    }
    public void SetVolume(float value, string group)
    {
        if (value > 0.01f)
        {
            float volume = Mathf.Log10(value) * 20;
            _mixer.SetFloat(group, volume);
        }
        else
        {
            _mixer.SetFloat(group, -80f);
        }
    }
    public void SetMasterVolume(float value)
    {
        SetVolume(value, "Master");
    }

    public void SetMusicVolume(float value)
    {
        SetVolume(value, "Music");
    }

    public void SetSFXVolume(float value)
    {
        SetVolume(value, "SFX");
    }

    public void PlayMusic(string name)
    {
        foreach (Sound _sound in musicSounds)
        {
            if (string.Equals(_sound.name,name))
            {
                if (musicSource.isPlaying) musicSource.Stop();
                musicSource.clip = _sound.clip;
                musicSource.Play();
                return;
            }
        }
        Debug.LogWarning($"Music Not Found in my list: {name}");
    }

    public void PlaySFX(string name)
    {
        foreach (Sound _sound in sfxSounds)
        {
            if (string.Equals(_sound.name,name))
            {
                // if (sfxSource.isPlaying) sfxSource.Stop();
                sfxSource.PlayOneShot(_sound.clip);
                return;
            }
        }
        Debug.LogWarning($"SFX sound Not Found in my list: {name}");
    }

    // Stop All Audio Source !!!!!
    public void StopAllAudioSource()
    {
        if (musicSource.isPlaying) musicSource.Stop();
        if (sfxSource.isPlaying) sfxSource.Stop();
    }

}
