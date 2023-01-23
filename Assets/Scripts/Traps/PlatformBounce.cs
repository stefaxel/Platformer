using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBounce : MonoBehaviour
{
    [SerializeField] float upwardsForce;
    private Animator bounceAnimator;

    bool bounceAnimation;

    private void Start()
    {
        bounceAnimator = GetComponent<Animator>();
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            bounceAnimator.SetBool("bounce player", true);
            collision.rigidbody.AddForce(Vector2.up * upwardsForce, ForceMode2D.Impulse);
            SetIdleAnimation();
        }
    }

    private void SetIdleAnimation()
    {
        bounceAnimator.SetBool("bounce player", false);
    }
}
