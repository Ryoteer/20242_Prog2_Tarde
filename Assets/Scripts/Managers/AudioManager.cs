using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("<color=orange>Audio</color>")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioSource _source;

    private float _masterVolume = 0.0f;
    public float MasterVolume 
    {
        get { return _masterVolume; }
        set { _masterVolume = value; }
    }

    private float _musicVolume = 0.0f;
    public float MusicVolume
    {
        get { return _musicVolume; }
        set { _musicVolume = value; }
    }

    private float _sfxVolume = 0.0f;
    public float SfxVolume
    {
        get { return _sfxVolume; }
        set { _sfxVolume = value; }
    }

    public void SetMasterVolume(float value)
    {
        _masterVolume = value;

        _mixer.SetFloat("Master", Mathf.Log10(value) * 20.0f);
    }

    public void SetMusicVolume(float value)
    {
        _musicVolume = value;

        _mixer.SetFloat("Music", Mathf.Log10(value) * 20.0f);
    }

    public void SetSFXVolume(float value)
    {
        _sfxVolume = value;

        _mixer.SetFloat("SFX", Mathf.Log10(value) * 20.0f);
    }

    public void PlayAudio(AudioClip clip)
    {
        if(clip != _source.clip)
        {
            _source.clip = clip;

            _source.Play();
        }
    }

    public void PauseAudio()
    {
        _source.Pause();
    }

    public void StopAudio()
    {
        _source.Stop();
    }
}
