using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikehead : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float sightRange;
    [SerializeField] private float attackDelay;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int damage;
    private float attackTimer;
    private Vector3 destination;

    private bool isAttacking;

    private Vector3[] direction = new Vector3[4];

    private void OnEnable()
    {
        StopAttacking();
    }

    private void Update()
    {
        if (isAttacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackDelay)
                CheckForPlayer();
        }
    }

    private void CheckForPlayer()
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

    private void CalculateAttackDirection()
    {
        direction[0] = transform.right * sightRange;
        direction[1] = -transform.right * sightRange;
        direction[2] = transform.up * sightRange;
        direction[3] = -transform.up * sightRange;
    }

    private void StopAttacking()
    {
        destination = transform.position;
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            StopAttacking();
        }
    }
}
