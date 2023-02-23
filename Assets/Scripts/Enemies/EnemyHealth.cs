using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health;
    private int currentHealth;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        currentHealth = health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TakeDamage();
            //animator.SetTrigger("hit");
            if (health <= 0)
                this.gameObject.SetActive(false);
        }
    }

    private void TakeDamage()
    {
        if (currentHealth > 0)
        {
            animator.SetTrigger("hit");
            health--;

        }
    }
}
