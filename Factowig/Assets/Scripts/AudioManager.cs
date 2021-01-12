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
        TIMEOUT
    }

    public static AudioManager Instance = null;

    public static float musicVolume = 1f;
    public static float fxVolume = 1f;

    [SerializeField]
    private AudioSource BSOAudioSource = null;

    [SerializeField]
    private AudioSource FXAudioSource = null;

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
        FXAudioSource.volume = fxVolume;
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
        FXAudioSource.loop = loop;
        FXAudioSource.clip = fxEffects[(int)effect];
        FXAudioSource.Play();
    }

    public void StopSoundEffects()
    {
        FXAudioSource.Stop();
    }

    public FX GetPlayingFX()
    {
        if (FXAudioSource.isPlaying)
        {
            for (int i = 0; i < fxEffects.Length; ++i)
                if (fxEffects[i] == FXAudioSource.clip)
                    return (FX)i;
        }
        return FX.NONE;
    }
}