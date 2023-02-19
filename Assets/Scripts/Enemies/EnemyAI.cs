using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.InputSystem.XR;
using UnityEditor.Tilemaps;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Path path;
    [SerializeField] private float speed;
    [SerializeField] private float nextWaypointDistance;
    [SerializeField] private LayerMask whatIsPlayer;
    private int currentWaypoint = 0;

    [SerializeField] private float sightRange;
    //[SerializeField] private float attackRange;
    public bool playerInSight;
    public bool playerInAttack;

    public Color gizmoColor = Color.green;
    public bool showGizmos = true;

    private bool movingRight = true;
    [SerializeField] private Transform groundDetection;
    [SerializeField] private float rayDistance;
    [SerializeField] private float patrolSpeed;

    private bool reachedEndOfPath;
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

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
        playerInAttack = Physics2D.OverlapCircle(transform.position, nextWaypointDistance, whatIsPlayer);

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
            if (distanceToWaypoint < nextWaypointDistance)
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
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        Vector3 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector3 force = dir * speed * speedFactor * Time.deltaTime;

        rb.AddForce(force);

        //float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        //if (distance < nextWaypointDistance)
        //{
        //    currentWaypoint++;
        //}
    }

    private void AttackPlayer()
    {

    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        patrolSpeed *= -1;

    }

    private void OnDrawGizmos()
    {
        if(showGizmos)
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, nextWaypointDistance);
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nextWaypointDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, nextWaypointDistance);
    }
}
