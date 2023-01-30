using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointEnd : MonoBehaviour
{
    Collider2D boxCollider;

    private Animator checkpointEndAnimation;

    [SerializeField] private AudioClip checkpointAudio;

    private void Start()
    {
        boxCollider = GetComponent<Collider2D>();
        checkpointEndAnimation = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Reached end of level load next level");
            SoundManager.instance.PlaySound(checkpointAudio);
            checkpointEndAnimation.SetTrigger("end trigger");
            boxCollider.enabled = false;
        }
    }
}
