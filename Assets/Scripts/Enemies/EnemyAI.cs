using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem.XR;
using UnityEditor.Tilemaps;
using System;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfind references")]
    [SerializeField] protected Transform targetPosition;
    protected Seeker seeker;
    protected Rigidbody2D rb;
    public Path path;

    [Header("Enemy Pathfind Params")]
    [SerializeField] protected float speed;
    [SerializeField] protected Vector2 nextWaypointDistance;
    [SerializeField]protected float nextWaypointFloat;
    [SerializeField] protected LayerMask whatIsPlayer;
    protected int currentWaypoint = 0;
    [SerializeField] protected float sightRange;
    //[SerializeField] private float attackRange;
    protected bool playerInSight;
    protected bool playerInAttack;

    [Header("Gizmos")]
    public Color gizmoColor = Color.green;
    public Color attackGizmo = Color.red;
    public bool showGizmos = true;
    public bool showAttackGizmo = true;

    [Header("Patrol Settings")]
    [SerializeField] protected Transform groundDetection;
    [SerializeField] protected float rayDistance;
    [SerializeField] protected float patrolSpeed;
    protected bool isGoingRight = true;

    [Header("Attack Settings")]
    [SerializeField] protected Transform attackDetection;
    [SerializeField] protected Transform attackPoint;
    //private bool inAtotectedange;
    [SerializeField] protected Vector2 detectorSize;
    [SerializeField] protected Vector2 detectorOffset;
    [SerializeField] protected Vector2 attackPointSize;
    [SerializeField] protected Vector2 attackPointOffset;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float detectionDelay;

    [Header("Jump Settings")]
    [SerializeField] protected Transform jumpDetectorPlayer;
    [SerializeField] protected float jumpForce;
    [SerializeField] protected Vector2 jumpDetectorSize;
    [SerializeField] protected Vector2 jumpDetectorOffset;
    protected bool playerHasJumped;
    protected bool isEnemyonGround;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float rayDistanceJump;
    protected bool hasPlayerJumped;
    [SerializeField] protected float jumpDelay;
    protected float jumpTimer;


    protected bool reachedEndOfPath;
    protected bool facingRight = true;
    protected bool playerEncountered = false;
    protected bool wasFacingRight;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        nextWaypointFloat = nextWaypointDistance.x/2f;
        //StartCoroutine(PlayerDetection());
    }

    //IEnumerator PlayerDetection()
    //{
    //    yield return new WaitForSeconds(detectionDelay);
    //    AttackPlayer();
    //    StartCoroutine(PlayerDetection());
    //}

    protected virtual void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, targetPosition.position, OnPathComplete);
    }

    protected virtual void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        AIChecks();
    }

    protected virtual void AIChecks()
    {
        playerInSight = Physics2D.OverlapCircle(transform.position, sightRange, whatIsPlayer);
        playerInAttack = Physics2D.OverlapBox(transform.position, nextWaypointDistance, 0, whatIsPlayer);

        playerHasJumped = Physics2D.OverlapBox(jumpDetectorPlayer.position, jumpDetectorSize, 0, whatIsPlayer);
        //playerInAttack = Physics2D.OverlapCircle(transform.position, nextWaypointDistance, whatIsPlayer);
        //inAttackRange = Physics2D.OverlapBox((Vector2)attackDetection.position + detectorOffset, detectorSize, 0, whatIsPlayer);

        if (!playerInSight && !playerInAttack)
            Patrol();

        if(playerInSight && !playerInAttack)
        {
            ChasePlayer();
            Flip();
        }
          
        if (playerInSight && playerInAttack)
            AttackPlayer();

        if (playerHasJumped)
        {
            hasPlayerJumped = true;
        }

        if (playerInSight && playerHasJumped)
            EnemyJump();
        
    }

    protected virtual void Patrol()
    {
        if (playerEncountered && !playerInSight)
            Flip();

        rb.velocity = new Vector2(patrolSpeed * Time.deltaTime, rb.velocity.y);
        RaycastHit2D isGround = Physics2D.Raycast(groundDetection.position, Vector2.down, rayDistance);

        if (!isGround.collider)
            Flip(); 
    }

    protected virtual void ChasePlayer()
    {
        if (path == null)
            return;


        reachedEndOfPath = false;

        float distanceToWaypoint;
        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointFloat)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointFloat) : 1f;

        Vector3 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector3 force = dir * speed * speedFactor * Time.deltaTime;

        rb.AddForce(force);
    }

    protected virtual void EnemyJump()
    {
        jumpTimer += Time.deltaTime;
        if(jumpTimer > jumpDelay)
            rb.AddForce(Vector2.up * jumpForce);

        //if (IsGrounded())
        //{
        Debug.Log("player has jumped");
            

        Flip();
        //}

    }

    protected virtual void AttackPlayer()
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)attackDetection.position + detectorOffset, detectorSize, 0, whatIsPlayer);
        float distance;

        Flip();

        if (collider != null)
        {
            distance = Vector2.Distance(transform.position, targetPosition.position);
            Vector2 direction = targetPosition.position - transform.position;
            Vector3 force = direction * attackSpeed * Time.deltaTime;

            rb.AddForce(force);
            
            Collider2D attackCollider = Physics2D.OverlapBox((Vector2)attackPoint.position + detectorOffset, attackPointSize, 0, whatIsPlayer);
            if (attackCollider != null)
            {
                attackCollider.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            }
        }
        else
        {
            Debug.Log("not in range");
        }

    }

    protected virtual void Flip()
    {
        wasFacingRight = facingRight;

        if (!playerInSight)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            patrolSpeed *= -1;
            facingRight = !facingRight; 
            isGoingRight = !isGoingRight;
        }

        if ((!wasFacingRight && playerEncountered && isGoingRight) || (!wasFacingRight && hasPlayerJumped && playerEncountered && IsGrounded()))
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            //facingRight= !facingRight;
            playerEncountered = false;
            hasPlayerJumped = false;
        }

        if (playerInSight || playerInAttack)
        {
            playerEncountered = true;
            if ((targetPosition.position.x > rb.position.x && transform.localScale.x < 0) ||
                (targetPosition.position.x < rb.position.x && transform.localScale.x > 0))
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                facingRight = !facingRight;
            }   
        }
        if (playerHasJumped && playerInSight)
            playerEncountered = true;


    }

    protected virtual bool IsGrounded()
    {
        isEnemyonGround = Physics2D.Raycast(transform.position, Vector2.down, rayDistanceJump, whatIsGround.value);
        return isEnemyonGround;
    }

    protected virtual void OnDrawGizmos()
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

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(attackPoint.position, attackPointSize);
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nextWaypointFloat);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, nextWaypointFloat);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, nextWaypointDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(jumpDetectorPlayer.position, jumpDetectorSize);
        Gizmos.color = Color.white;
        Vector3 direction = transform.TransformDirection(Vector2.down) * rayDistanceJump;
        Gizmos.DrawRay(transform.position, direction);
        
    }
}
