using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] private AudioSource soundEffect;

    void Awake()
    {
        soundEffect.playOnAwake = true;    
    }

    
}
