using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;
    [SerializeField] AudioSource[] sfxArray;

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

    public void PlaySoundTime(float _soundTime, AudioClip _sound)
    {
        source.PlayScheduled(_soundTime);
        source.PlayOneShot(_sound);
    }
}
