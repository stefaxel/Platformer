using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    private static readonly string firstStartUp = "FirstStartUp";
    private static readonly string musicPref = "MusicPref";
    private static readonly string effectsPref = "EffectsPref";
    private int firstStartUpInt;
    private float musicVolume;
    private float effectsVolume;

    public Slider musicSlider;
    public Slider effectsSlider;

    // Start is called before the first frame update
    void Start()
    {
        firstStartUpInt = PlayerPrefs.GetInt(firstStartUp);

        if(firstStartUpInt == 0)
        {
            musicVolume = 0.15f;
            effectsVolume = 0.75f;

            musicSlider.value = musicVolume;
            effectsSlider.value = effectsVolume;

            PlayerPrefs.SetFloat(musicPref, musicVolume);
            PlayerPrefs.SetFloat(effectsPref, effectsVolume);
            PlayerPrefs.SetInt(firstStartUp, 1);
        }
        else
        {
            musicVolume = PlayerPrefs.GetFloat(musicPref);
            musicSlider.value = musicVolume;

            effectsVolume = PlayerPrefs.GetFloat(effectsPref);
            effectsSlider.value = effectsVolume;
        }
    }

   public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(musicPref, musicSlider.value);
        PlayerPrefs.SetFloat(effectsPref, effectsSlider.value);
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
        {
            SaveSoundSettings();
        }
    }
}
