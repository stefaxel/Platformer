using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Threading;

public class RangedEnemyAI : EnemyAI
{
    [Header("Projectile Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] GameObject[] bullet;
    [SerializeField] private float attackDelay;
    private float attackDelayTimer;
    
    //bool playerJumpedToPlatform;
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
        base.EnemyJump();
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
        base.OnDrawGizmos();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
}
