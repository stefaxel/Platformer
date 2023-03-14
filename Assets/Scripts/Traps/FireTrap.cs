using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : DamageScript
{
    [SerializeField] float onTime;
    [SerializeField] float offTime;

    private bool isActive;
    private bool fireIsActive = false;
    private bool canTakeDamage;
    private bool animationActive = false;

    private Animator fireAnimation;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireAudio;
    [SerializeField] private float volume;

    void Start()
    {
        fireAnimation = GetComponent<Animator>();
        StartCoroutine(FireTrapTrigger());
    }

    //Coroutine that handles animation and the active time of the fire trap
    private IEnumerator FireTrapTrigger()
    {
        if(!isActive)
        {
            if (animationActive)
            {
                fireAnimation.SetBool("fire active", false);
                animationActive = false;
            }   
            canTakeDamage = false;
            yield return new WaitForSeconds(offTime);
            fireIsActive = true;
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(fireAudio, volume);
            }
            isActive = true;
        }

        if(isActive)
        {
            if (!animationActive)
            {
                fireAnimation.SetBool("fire active", true);
                animationActive = true;
            }
            
            canTakeDamage = true;
            yield return new WaitForSeconds(onTime);
            isActive = false;
            fireIsActive = false;
            audioSource.Stop();
        }

        if (!fireIsActive)
            StartCoroutine(FireTrapTrigger());
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            if (isActive && canTakeDamage)
            {
                collision.GetComponent<PlayerHealth>().TakeDamage(damage);
                Debug.Log("Damage taken = " + damage);
            }
        }
    }
}
