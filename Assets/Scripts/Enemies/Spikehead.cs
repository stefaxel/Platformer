using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spikehead : DamageScript
{
    [SerializeField] protected float speed;
    [SerializeField] protected float sightRange;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected LayerMask playerLayer;
    //[SerializeField] protected int damage;
    [SerializeField] protected float enemyHealth;
    protected float attackTimer;
    protected Vector3 destination;

    protected bool isAttacking;

    [SerializeField] protected AudioClip audioImpact;

    protected Vector3[] direction = new Vector3[4];

    protected virtual void OnEnable()
    {
        StopAttacking();
    }

    protected virtual void Update()
    {
        if (isAttacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackDelay)
                CheckForPlayer();
        }

        if(enemyHealth <= 0)
            Destroy(gameObject);
    }

    protected virtual void CheckForPlayer()
    {
        CalculateAttackDirection();

        for(int i = 0; i < direction.Length; i++)
        {
            Debug.DrawRay(transform.position, direction[i], Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction[i], sightRange, playerLayer);

            if(hit.collider != null && !isAttacking)
            {
                isAttacking = true;
                destination = direction[i];
            }
        }
    }

    protected virtual void CalculateAttackDirection()
    {
        direction[0] = transform.right * sightRange; //right
        direction[1] = -transform.right * sightRange; //left
        direction[2] = transform.up * sightRange; //up
        direction[3] = -transform.up * sightRange; //down
    }

    protected virtual void StopAttacking()
    {
        destination = transform.position;
        isAttacking = false;
    }

    //protected virtual void OnCollisionEnter2D(Collider2D collision)
    //{
            
    //}

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            SoundManager.instance.PlaySound(audioImpact);
            StopAttacking();
        }
        if (collision.gameObject.name == "Wall" || collision.gameObject.name == "Ground")
        {
            enemyHealth--;
            SoundManager.instance.PlaySound(audioImpact);
            StopAttacking();
        }
    }
}
