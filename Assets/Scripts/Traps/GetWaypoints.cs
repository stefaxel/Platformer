using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GetWaypoints : MonoBehaviour
{
    [SerializeField] GameObject[] waypoint;
    int currentWaypointIndex = 0;

    [SerializeField] float speed;

    
    void Update()
    {
        if (Vector2.Distance(waypoint[currentWaypointIndex].transform.position, transform.position) < 0.1f)
        {
            currentWaypointIndex++;
            if(currentWaypointIndex >= waypoint.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, 
            waypoint[currentWaypointIndex].transform.position, speed * Time.deltaTime);
    }
}
