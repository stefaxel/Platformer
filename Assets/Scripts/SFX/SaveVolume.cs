using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveVolume : MonoBehaviour
{
    public static SaveVolume instance { get; private set; }
    private static readonly string musicPref = "MusicPref";
    private static readonly string effectsPref = "EffectsPref";

    private float musicVolume;
    private float effectsVolume;

    private AudioSource source;

    [SerializeField] AudioSource[] fanSfx;
    [SerializeField] AudioSource[] fireSfx;
    [SerializeField] AudioSource[] platformSfx;
    [SerializeField] AudioSource levelMusic;

    
    void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        ContinueSettings();
    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }

    private void ContinueSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(musicPref);

        effectsVolume = PlayerPrefs.GetFloat(effectsPref);

        source.volume = PlayerPrefs.GetFloat(effectsPref);

        levelMusic.volume = musicVolume;

        for (int i = 0; i < fanSfx.Length; i++)
        {
            fanSfx[i].volume = effectsVolume;
        }

        for (int i = 0; i < fireSfx.Length; i++)
        {
            fireSfx[i].volume = effectsVolume;
        }

        for (int i = 0; i < platformSfx.Length; i++)
        {
            platformSfx[i].volume = effectsVolume;
        }
    }
}
