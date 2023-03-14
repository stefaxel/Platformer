using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBounce : MonoBehaviour
{
    [SerializeField] float upwardsForce;
    
    private Animator bounceAnimator;
    bool bounceAnimation = false;

    [SerializeField] private AudioClip bounceAudio;

    private void Start()
    {
        bounceAnimator = GetComponent<Animator>();
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if(!bounceAnimation)
            {
                SoundManager.instance.PlaySound(bounceAudio);

                bounceAnimation = true;
                bounceAnimator.SetTrigger("bounce");
                bounceAnimation = false;
            }
            collision.rigidbody.AddForce(transform.up * upwardsForce, ForceMode2D.Impulse);
        }
    }
}
