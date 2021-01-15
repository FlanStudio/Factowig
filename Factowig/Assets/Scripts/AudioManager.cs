using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum BSO
    {
        NONE = -1,
        LEVEL,
        SETTINGS
    }

    public enum FX
    {
        NONE = -1,
        TICK,
        PLACE,
        PICK,
        WORKING,
        TIMEOUT,
        BELT,
        THROW,
        CLICK,
        WIGS,
        NEWRECIPE,
        WRONGDELIVERY,
        CORRECTDELIVERY
    }

    public static AudioManager Instance = null;

    public static float musicVolume = 1f;
    public static float fxVolume = 1f;

    [SerializeField]
    private AudioSource BSOAudioSource = null;

    [SerializeField]
    private AudioSource[] FXAudioSources = null;

    [SerializeField]
    private AudioClip[] BSOs = null;

    [SerializeField]
    private AudioClip[] fxEffects = null;

    private void Awake()
    {
        Instance = this;

        musicVolume = PlayerPrefs.GetFloat("musicVolume");
        fxVolume = PlayerPrefs.GetFloat("fxVolume");

        ApplyVolumesToSources();

        PlayBSO(BSO.LEVEL);
    }

    public void ApplyVolumesToSources()
    {
        BSOAudioSource.volume = musicVolume;
        foreach (AudioSource audioSource in FXAudioSources)
            audioSource.volume = fxVolume;
    }

    public void PlayBSO(BSO bso)
    {
        if(BSOs[(int)bso] != null && BSOAudioSource != null)
        {
            BSOAudioSource.clip = BSOs[(int)bso];
            BSOAudioSource.Play();
        }
    }

    public void PauseBSO()
    {
        BSOAudioSource.Pause();
    }

    public void PlaySoundEffect(FX effect, bool loop = false)
    {
        if (FXAudioSources.Length == 0)
            return;

        bool played = false;
        foreach(AudioSource fx in FXAudioSources)
        {
            if (!fx.isPlaying)
            {
                fx.loop = loop;
                fx.clip = fxEffects[(int)effect];
                fx.Play();
                played = true;
                break;
            }
        }

        if(!played)
        {
            FXAudioSources[0].Stop();
            FXAudioSources[0].loop = loop;
            FXAudioSources[0].clip = fxEffects[(int)effect];
            FXAudioSources[0].Play();
        }
    }

    public void StopSoundEffects()
    {
        foreach(AudioSource fx in FXAudioSources)
            fx.Stop();
    }

    public bool IsPlayingFX(FX fx)
    {
        foreach(AudioSource audioSource in FXAudioSources)
        {
            if(audioSource.isPlaying)
            {               
                if (fxEffects[(int)fx] == audioSource.clip)
                    return true;
            }
        }

        return false;
    }
}