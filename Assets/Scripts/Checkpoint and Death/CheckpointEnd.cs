using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;


//Script that plays a sound and loads the next level once the player has completed the level
public class CheckpointEnd : MonoBehaviour
{
    Collider2D boxCollider;

    private Animator checkpointEndAnimation;

    [SerializeField] private AudioClip checkpointAudio;

    private bool levelCompleted = false;
    private void Start()
    {
        boxCollider = GetComponent<Collider2D>();
        checkpointEndAnimation = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !levelCompleted)
        {
            SoundManager.instance.PlaySound(checkpointAudio);
            checkpointEndAnimation.SetTrigger("end trigger");
            boxCollider.enabled = false;

            Invoke("LoadNextScene", 1f);
            
            levelCompleted = true;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
