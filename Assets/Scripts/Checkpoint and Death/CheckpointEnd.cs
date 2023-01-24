using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointEnd : MonoBehaviour
{
    private Animator checkpointEndAnimation;

    private void Start()
    {
        checkpointEndAnimation = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Reached end of level load next level");
            checkpointEndAnimation.SetTrigger("end trigger");
        }
    }
}
