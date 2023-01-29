using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FanTrap : MonoBehaviour
{
    [SerializeField] float onTime;
    [SerializeField] float offTime;
    [SerializeField] float forceApplied;

    private Animator fanAnimation;

    private bool fanIsActive = true;
    private bool isActive = true;
    private bool fanCanBlowPlayer;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fanAudio;
    [SerializeField] private float volume;

    private void Start()
    {
        fanAnimation = GetComponent<Animator>();
        StartCoroutine(FanTrapTrigger());
    }

    private void Update()
    {
        if(!fanIsActive)
            StartCoroutine(FanTrapTrigger());
    }

    IEnumerator FanTrapTrigger()
    {
        if (!isActive)
        {
            fanAnimation.SetBool("activated", false);
            fanCanBlowPlayer = false;
            yield return new WaitForSeconds(offTime);
            fanIsActive = true;
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(fanAudio, volume);
            }
            isActive = true;
        }

        if (isActive)
        {
            fanAnimation.SetBool("activated", true);
            fanCanBlowPlayer = true;
            yield return new WaitForSeconds(onTime);
            isActive = false;
            fanIsActive = false;
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (isActive && fanCanBlowPlayer)
            {
                collision.GetComponent<Rigidbody2D>().AddForce(transform.up * forceApplied, ForceMode2D.Impulse);
            }
        }
    }

}
