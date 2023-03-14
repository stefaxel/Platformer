using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem.XR;
using System;
using UnityEngine.UIElements;

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
    [SerializeField] private Transform attackPoint;
    [SerializeField] protected Vector2 detectorSize;
    [SerializeField] protected Vector2 detectorOffset;
    [SerializeField] private Vector2 attackPointSize;
    [SerializeField] private Vector2 attackPointOffset;
    [SerializeField] private float attackSpeed;
    [SerializeField] protected float detectionDelay;

    [Header("Jump Settings")]
    [SerializeField] protected Transform jumpDetectorPlayer;
    [SerializeField] protected float jumpForce;
    [SerializeField] protected Vector2 jumpDetectorSize;
    [SerializeField] protected Vector2 jumpDetectorOffset;
    protected bool playerHasJumped;
    protected bool isEnemyonGround;
    protected bool playerIsAbove;
    [SerializeField] protected Vector2 playerAboveJumpSize;
    [SerializeField] protected Transform playerAboveTransform;
    protected bool playerIsBelow;
    [SerializeField] protected Vector2 playerBelowJumpSize;
    [SerializeField] protected Transform playerBelowTransform;
    [SerializeField] protected Transform groundInFront;
    [SerializeField] protected Vector2 groundInFrontSize;
    protected bool isGroundInFront;
    [SerializeField] protected LayerMask whatIsInFront;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float rayDistanceJump;
    protected bool hasPlayerJumped;
    [SerializeField] protected float jumpDelay;
    protected float jumpTimer;

    protected bool reachedEndOfPath;
    protected bool facingRight = true;
    protected bool playerEncountered = false;
    protected bool wasFacingRight;
    protected float enemyBounds = -20f;

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        nextWaypointFloat = nextWaypointDistance.x/2f;
    }

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

        if(transform.position.y <= enemyBounds)
            this.gameObject.SetActive(false);
    }

    protected virtual void AIChecks()
    {
        playerInSight = Physics2D.OverlapCircle(transform.position, sightRange, whatIsPlayer); //Checks to see if the player is in sight for pathfinding
        playerInAttack = Physics2D.OverlapBox(transform.position, nextWaypointDistance, 0, whatIsPlayer); //Checks to see if the player is in attack range

        // Jumping conditions
        playerHasJumped = Physics2D.OverlapBox(jumpDetectorPlayer.position, jumpDetectorSize, 0, whatIsPlayer);
        playerIsAbove = Physics2D.OverlapBox(playerAboveTransform.position, playerAboveJumpSize, 0, whatIsPlayer);
        playerIsBelow = Physics2D.OverlapBox(playerBelowTransform.position, playerBelowJumpSize, 0, whatIsPlayer);
        isGroundInFront = Physics2D.OverlapBox(groundInFront.position, groundInFrontSize, 0, whatIsInFront);

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

        if (playerIsAbove && IsGrounded())
            EnemyJump();

        if (playerIsBelow && IsGrounded())
            EnemyJump();

        if (isGroundInFront)
            EnemyJump();

    }

    //Patroling state for the AI
    protected virtual void Patrol()
    {
        animator.SetBool("moving", true);

        if (playerEncountered && !playerInSight)
            Flip();

        rb.velocity = new Vector2(patrolSpeed * Time.deltaTime, rb.velocity.y);
        RaycastHit2D isGround = Physics2D.Raycast(groundDetection.position, Vector2.down, rayDistance);

        //If the raycast doesn't detect ground, it will flip the sprite and change direction
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

    //Conditions that allow the AI to Jump when chasing the player
    protected virtual void EnemyJump()
    {
        RaycastHit2D isGround = Physics2D.Raycast(transform.position, Vector2.down, rayDistance);

        jumpTimer += Time.deltaTime;
        if(jumpTimer > jumpDelay)
            rb.AddForce(Vector2.up * jumpForce);

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

        Flip();
        

    }

    protected virtual void AttackPlayer()
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)attackDetection.position + detectorOffset, detectorSize, 0, whatIsPlayer);
        float distance;

        Flip();

        //If the player collides with the collider the enemy can attack
        if (collider != null)
        {
            //Allows the AI to know which direction to move in
            distance = Vector2.Distance(transform.position, targetPosition.position);
            Vector2 direction = targetPosition.position - transform.position;
            Vector3 force = direction * attackSpeed * Time.deltaTime;

            rb.AddForce(force);
            animator.SetTrigger("attack");
            
            Collider2D attackCollider = Physics2D.OverlapBox((Vector2)attackPoint.position + detectorOffset, attackPointSize, 0, whatIsPlayer);
            if (attackCollider != null)
            {
                attackCollider.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            }
        }
        else
        {
            animator.SetBool("moving", false);
        }

    }

    protected virtual void Flip()
    {
        // Makes the nemy face the correct direction once the player has escaped its range
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

    //Checks to see if the enemy is grounded, for the patrol system which allows the enmy to flip
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

            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, nextWaypointFloat);
        }
        if (showAttackGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackDetection.position, detectorSize);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(attackPoint.position, attackPointSize);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(attackDetection.position, detectorSize);
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(playerAboveTransform.position, playerAboveJumpSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(playerBelowTransform.position, playerBelowJumpSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundInFront.position, groundInFrontSize);

    }
}
