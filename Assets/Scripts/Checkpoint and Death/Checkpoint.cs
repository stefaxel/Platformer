using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    PlayerPosition playerPosition;
    Collider2D boxCollider;
    Animator checkpointAnimation;

    [SerializeField] private AudioClip checkpointAudio;

    private void Awake()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPosition>();
    }

    private void Start()
    {
        checkpointAnimation = GetComponent<Animator>();
        boxCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks if the player has collided with a trigger to update the respawn point
        if (collision.CompareTag("Player"))
        {
            checkpointAnimation.SetTrigger("trigger");
            SoundManager.instance.PlaySound(checkpointAudio);
            playerPosition.UpdateCheckpoint(transform.position);
            boxCollider.enabled = false; //Prevents the player from colliding with the trigger multiple times
        }
    }
}
