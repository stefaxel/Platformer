using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBounce : MonoBehaviour
{
    [SerializeField] float upwardsForce;
    private Animator bounceAnimator;

    bool bounceAnimation = false;

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
                bounceAnimation = true;
                bounceAnimator.SetTrigger("bounce");
                bounceAnimation = false;
            }
            //bounceAnimator.SetTrigger("bounce");
            collision.rigidbody.AddForce(Vector2.up * upwardsForce, ForceMode2D.Impulse);
        }
    }
}
