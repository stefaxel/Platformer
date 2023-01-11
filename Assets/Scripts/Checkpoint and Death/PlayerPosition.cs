using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    Vector2 checkpointPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        checkpointPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCheckpoint(Vector2 position)
    {
        checkpointPosition = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "FallDetector")
        {
            transform.position = checkpointPosition;
        }
    }
}
