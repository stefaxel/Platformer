using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    [SerializeField] private AudioClip buttonAudio;

    public void OnButtonPress()
    {
        SoundManager.instance.PlaySound(buttonAudio);
    }
}
