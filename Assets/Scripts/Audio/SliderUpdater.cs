using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdater : MonoBehaviour
{
    [Header("<color=orange>Audio</color>")]
    [Range(0.0f, 1.0f)][SerializeField] private float _masterValue = 1.0f;
    [Range(0.0f, 1.0f)][SerializeField] private float _musicValue = 0.375f;
    [Range(0.0f, 1.0f)][SerializeField] private float _sfxValue = 0.75f;

    [Header("<color=orange>UI</color>")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Awake()
    {
        if(AudioManager.Instance.MasterVolume != 0.0f)
        {
            _masterSlider.value = AudioManager.Instance.MasterVolume;
            AudioManager.Instance.SetMasterVolume(AudioManager.Instance.MasterVolume);
        }
        else
        {
            _masterSlider.value = _masterValue;
            AudioManager.Instance.SetMasterVolume(_masterValue);
        }

        if (AudioManager.Instance.MusicVolume != 0.0f)
        {
            _musicSlider.value = AudioManager.Instance.MusicVolume;
            AudioManager.Instance.SetMusicVolume(AudioManager.Instance.MusicVolume);
        }
        else
        {
            _musicSlider.value = _musicValue;
            AudioManager.Instance.SetMusicVolume(_musicValue);
        }

        if (AudioManager.Instance.SfxVolume != 0.0f)
        {
            _sfxSlider.value = AudioManager.Instance.SfxVolume;
            AudioManager.Instance.SetSFXVolume(AudioManager.Instance.SfxVolume);
        }
        else
        {
            _sfxSlider.value = _sfxValue;
            AudioManager.Instance.SetSFXVolume(_sfxValue);
        }
    }

    public void SetMasterVolume(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }
}
