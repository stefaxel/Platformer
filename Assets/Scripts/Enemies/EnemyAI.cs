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
    [SerializeField] private Transform targetPosition;
    private Seeker seeker;
    private Rigidbody2D rb;
    public Path path;

    [Header("Enemy Pathfind Params")]
    [SerializeField] private float speed;
    [SerializeField] private Vector2 nextWaypointDistance;
    private float nextWaypointFloat;
    [SerializeField] private LayerMask whatIsPlayer;
    private int currentWaypoint = 0;
    [SerializeField] private float sightRange;
    //[SerializeField] private float attackRange;
    public bool playerInSight;
    public bool playerInAttack;

    [Header("Gizmos")]
    public Color gizmoColor = Color.green;
    public Color attackGizmo = Color.red;
    public bool showGizmos = true;
    public bool showAttackGizmo = true;

    [Header("Patrol Settings")]
    [SerializeField] private Transform groundDetection;
    [SerializeField] private float rayDistance;
    [SerializeField] private float patrolSpeed;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackDetection;
    [SerializeField] private Transform attackPoint;
    //private bool inAttackRange;
    [SerializeField] private Vector2 detectorSize;
    [SerializeField] private Vector2 detectorOffset;
    [SerializeField] private Vector2 attackPointSize;
    [SerializeField] private Vector2 attackPointOffset;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float detectionDelay;

    private bool reachedEndOfPath;
    public bool facingRight = true;
    public bool playerEncountered = false;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        nextWaypointFloat = nextWaypointDistance.x/2.5f;
        //StartCoroutine(PlayerDetection());
    }

    //IEnumerator PlayerDetection()
    //{
    //    yield return new WaitForSeconds(detectionDelay);
    //    AttackPlayer();
    //    StartCoroutine(PlayerDetection());
    //}

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, targetPosition.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AIChecks();
    }

    private void AIChecks()
    {
        playerInSight = Physics2D.OverlapCircle(transform.position, sightRange, whatIsPlayer);
        playerInAttack = Physics2D.OverlapBox(transform.position, nextWaypointDistance, 0, whatIsPlayer);
        //playerInAttack = Physics2D.OverlapCircle(transform.position, nextWaypointDistance, whatIsPlayer);
        //inAttackRange = Physics2D.OverlapBox((Vector2)attackDetection.position + detectorOffset, detectorSize, 0, whatIsPlayer);

        if (!playerInSight && !playerInAttack)
            Patrol();

        if(playerInSight && !playerInAttack)
            ChasePlayer();
        

        if (playerInSight && playerInAttack)
            AttackPlayer();
        
    }

    private void Patrol()
    {
        rb.velocity = new Vector2(patrolSpeed * Time.deltaTime, rb.velocity.y);
        RaycastHit2D isGround = Physics2D.Raycast(groundDetection.position, Vector2.down, rayDistance);

        if (!isGround.collider)
            Flip();

        if (playerEncountered && !playerInSight)
            Flip();
        
    }

    private void ChasePlayer()
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

    private void AttackPlayer()
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

    private void Flip()
    {
        bool wasFacingRight = facingRight;

        if (!playerInSight)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            patrolSpeed *= -1;
            facingRight = !facingRight; 
        }

        if (!wasFacingRight && playerEncountered)
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            playerEncountered = false;
        }

        if (playerInSight || playerInAttack)
        {
            playerEncountered = true;
            if (targetPosition.position.x > rb.position.x && transform.localScale.x < 0 ||
                targetPosition.position.x < rb.position.x && transform.localScale.x > 0)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                facingRight = !facingRight;
            }
                
        }
    }

    private void OnDrawGizmos()
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

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nextWaypointFloat);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, nextWaypointFloat);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, nextWaypointDistance);
        
    }
}
