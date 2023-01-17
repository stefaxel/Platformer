using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    Vector2 checkpointPosition;
    [HideInInspector] public Vector2 respawnPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        checkpointPosition = transform.position;
    }

    public void UpdateCheckpoint(Vector2 position)
    {
        checkpointPosition = position;
        respawnPlayer = checkpointPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FallDetector")
        {
            transform.position = checkpointPosition;
        }
    }
}
