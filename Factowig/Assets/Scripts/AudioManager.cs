using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum FX
    {
        NONE = -1,
        TICK
    }

    public static AudioManager Instance = null;

    public static float musicVolume = 1f;
    public static float fxVolume = 1f;

    [SerializeField]
    private AudioSource BSOAudioSource = null;

    [SerializeField]
    private AudioSource FXAudioSource = null;

    [SerializeField]
    private AudioClip settingsBSO = null;

    [SerializeField]
    private AudioClip[] fxEffects = null;

    private void Awake()
    {
        Instance = this;

        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        fxVolume = PlayerPrefs.GetFloat("fxVolume");

        ApplyVolumesToSources();
    }

    public void ApplyVolumesToSources()
    {
        BSOAudioSource.volume = musicVolume;
        FXAudioSource.volume = fxVolume;
    }

    public void PlaySettingsBSO()
    {
        if(settingsBSO != null && BSOAudioSource != null)
        {
            BSOAudioSource.clip = settingsBSO;
            BSOAudioSource.Play();
        }
    }

    public void PlaySoundEffect(FX effect)
    {
        FXAudioSource.clip = fxEffects[(int)effect];
        FXAudioSource.Play();
    }
}