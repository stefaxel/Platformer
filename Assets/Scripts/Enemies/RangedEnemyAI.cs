using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RangedEnemyAI : EnemyAI
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bullet;
    bool playerIsAbove;
    [SerializeField] private Vector2 playerAboveJumpSize;
    [SerializeField] private Transform playerAboveTransform;
    bool playerIsBelow;
    [SerializeField] private Vector2 playerBelowJumpSize;
    [SerializeField] private Transform playerBelowTransform;

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdatePath()
    {
        base.UpdatePath();
    }

    protected override void OnPathComplete(Path p)
    {
        base.OnPathComplete(p);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void AIChecks()
    {
        base.AIChecks();
        playerIsAbove = Physics2D.OverlapBox(playerAboveTransform.position, playerAboveJumpSize, 0, whatIsPlayer);

        playerIsBelow = Physics2D.OverlapBox(playerBelowTransform.position, playerBelowJumpSize, 0, whatIsPlayer);

        if (playerIsAbove && IsGrounded())
            EnemyJump();

        if(playerIsBelow && IsGrounded())
            EnemyJump();
    }

    protected override void Patrol()
    {
        base.Patrol();
    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();
    }

    protected override void EnemyJump()
    {
        RaycastHit2D isGround = Physics2D.Raycast(groundDetection.position, Vector2.down, rayDistance);

        if (playerIsAbove)
            rb.AddForce(Vector2.up * jumpForce);


        if (playerIsBelow && !isGround.collider)
            rb.AddForce(Vector2.up * 50f);

    }

    protected override void AttackPlayer()
    {
        Collider2D collider = Physics2D.OverlapBox(attackDetection.position, detectorSize, 0, whatIsPlayer);
        
        Flip();

        if (collider != null)
        {
            Debug.Log("Shoot bullets");
            bullet.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("not in range");
        }
    }

    protected override void Flip()
    {
        base.Flip();
    }

    protected override bool IsGrounded()
    {
        return base.IsGrounded();
    }

    protected override void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, nextWaypointFloat);
        }
        if (showAttackGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackDetection.position, detectorSize);
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(playerAboveTransform.position, playerAboveJumpSize);

        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(playerBelowTransform.position, playerBelowJumpSize);
    }
}
