using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveVolume : MonoBehaviour
{
    private static readonly string musicPref = "MusicPref";
    private static readonly string effectsPref = "EffectsPref";

    private float musicVolume;
    private float effectsVolume;

    [SerializeField] AudioSource[] fanSfx;
    [SerializeField] AudioSource[] fireSfx;
    [SerializeField] AudioSource[] platformSfx;
    [SerializeField] AudioSource levelMusic;


    void Awake()
    {
        ContinueSettings();
    }

    private void ContinueSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(musicPref);

        effectsVolume = PlayerPrefs.GetFloat(effectsPref);

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
