using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Threading;

public class RangedEnemyAI : EnemyAI
{
    [SerializeField] private Transform firePoint;
    [SerializeField] GameObject[] bullet;
    bool playerIsAbove;
    [SerializeField] private Vector2 playerAboveJumpSize;
    [SerializeField] private Transform playerAboveTransform;
    bool playerIsBelow;
    [SerializeField] private Vector2 playerBelowJumpSize;
    [SerializeField] private Transform playerBelowTransform;

    [SerializeField] private Transform groundInFront;
    [SerializeField] private Vector2 groundInFrontSize;
    bool isGroundInFront;
    [SerializeField] private LayerMask whatIsInFront;

    [SerializeField] private float attackDelay;
    private float attackDelayTimer;
    
    bool playerJumpedToPlatform;
    //RaycastHit2D isGroundEdge;

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

        isGroundInFront = Physics2D.OverlapBox(groundInFront.position, groundInFrontSize, 0, whatIsInFront);


        if (playerIsAbove && IsGrounded())
            EnemyJump();

        if(playerIsBelow && IsGrounded())
            EnemyJump();

        if (isGroundInFront)
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
        RaycastHit2D isGround = Physics2D.Raycast(transform.position, Vector2.down, rayDistance);
        
        if (playerIsAbove)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }
            
        if (playerIsBelow && !isGround.collider)
        {
            rb.AddForce(Vector2.up * 50f);
        }

        if (isGroundInFront)
            rb.AddForce(Vector2.up * 150f);

    }

    protected override void AttackPlayer()
    {
        Collider2D collider = Physics2D.OverlapBox(attackDetection.position, detectorSize, 0, whatIsPlayer);
        
        Flip();

        if (collider != null)
        {
            bullet[FindBullets()].transform.position = firePoint.position;
            
            attackDelayTimer += Time.deltaTime;
            if(attackDelayTimer >= attackDelay)
            {
                bullet[FindBullets()].SetActive(true);
                attackDelayTimer = 0;
            }
        }
        else
        {
            Debug.Log("not in range");
        }
    }

    private int FindBullets()
    {
        for(int i = 0; i< bullet.Length; i++)
        {
            if (!bullet[i].activeInHierarchy)
                return i;
        }
        return 0;
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(playerBelowTransform.position, playerBelowJumpSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundInFront.position, groundInFrontSize);
    }
}
