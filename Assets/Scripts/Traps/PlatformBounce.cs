using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBounce : MonoBehaviour
{
    [SerializeField] float upwardsForce;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.rigidbody.AddForce(Vector2.up * upwardsForce, ForceMode2D.Impulse);
        }
    }
}
