using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private static readonly string firstStartUp = "FirstStartUp";
    private static readonly string musicPref = "MusicPref";
    private static readonly string effectsPref = "EffectsPref";
    private int firstStartUpInt;
    private float musicVolume;
    private float effectsVolume;

    public Slider musicSlider;
    public Slider effectsSlider;

    public static SoundManager instance { get; private set; }
    private AudioSource source;
    [SerializeField] AudioSource[] fanSfx;
    [SerializeField] AudioSource[] fireSfx;
    [SerializeField] AudioSource[] platformSfx;
    [SerializeField] AudioSource levelMusic;

    void Start()
    {
        firstStartUpInt = PlayerPrefs.GetInt(firstStartUp);

        if (firstStartUpInt == 0)
        {
            musicVolume = 0.15f;
            effectsVolume = 0.75f;

            musicSlider.value = musicVolume;
            effectsSlider.value = effectsVolume;

            PlayerPrefs.SetFloat(musicPref, musicVolume);
            PlayerPrefs.SetFloat(effectsPref, effectsVolume);
            PlayerPrefs.SetInt(firstStartUp, -1);
        }
        else
        {
            musicVolume = PlayerPrefs.GetFloat(musicPref);
            musicSlider.value = musicVolume;

            effectsVolume = PlayerPrefs.GetFloat(effectsPref);
            effectsSlider.value = effectsVolume;
        }
    }

    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        
    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(musicPref, musicSlider.value);
        PlayerPrefs.SetFloat(effectsPref, effectsSlider.value);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        levelMusic.volume = musicSlider.value;

        source.volume = effectsSlider.value;

        for (int i = 0; i < fanSfx.Length; i++)
        {
            fanSfx[i].volume = effectsSlider.value;
        }

        for (int i = 0; i < fireSfx.Length; i++)
        {
            fireSfx[i].volume = effectsSlider.value;
        }

        for (int i = 0; i < platformSfx.Length; i++)
        {
            platformSfx[i].volume = effectsSlider.value;
        }
    }

    //public void PlaySoundTime(float _soundTime, AudioClip _sound)
    //{
    //    source.PlayScheduled(_soundTime);
    //    source.PlayOneShot(_sound);
    //}
}
