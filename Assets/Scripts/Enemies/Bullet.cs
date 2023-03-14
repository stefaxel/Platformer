using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float bulletLifetime;
    [SerializeField] int damage;
    private float bulletLifetimeTimer;

    // Update is called once per frame
    void FixedUpdate()
    {
        bulletLifetimeTimer += Time.deltaTime;

        float movementSpeed = speed * Time.deltaTime;

        transform.Translate(movementSpeed, 0, 0);

        //Destorys the bullet after a certain time set in the inspector
        if(bulletLifetimeTimer >= bulletLifetime)
        {
            this.gameObject.SetActive(false);
            bulletLifetimeTimer = 0;
        }
    }

    //Collision Detection for the bullet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            this.gameObject.SetActive(false);
        }
    }
}
