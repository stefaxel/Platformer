using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float waitForSecondsDown;
    [SerializeField] float waitForSecondsUp;
    [SerializeField] float speed;
    bool isFalling = false;

    Vector2 initialPos;
    BoxCollider2D boxCollider;
    Rigidbody2D rb;

    bool platformMovingBack;

    private Animator fallingAnimation;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip motorAudio;
    [SerializeField] private float volume;

    void Awake()
    {
        fallingAnimation = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        initialPos = transform.position;
        audioSource.PlayOneShot(motorAudio, volume);
    }

    private void Update()
    {
        if (platformMovingBack)
            transform.position = Vector2.MoveTowards(transform.position, initialPos, speed * Time.deltaTime);

        if (transform.position.y == initialPos.y)
            platformMovingBack = false;

        if (!audioSource.isPlaying && !isFalling)
        {
            audioSource.PlayOneShot(motorAudio, volume);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player" && !platformMovingBack)
        {
            StartCoroutine(WaitForPlatformFall(waitForSecondsDown));
        }
    }

    private IEnumerator WaitForPlatformFall(float time)
    {
        yield return new WaitForSeconds(time);

        fallingAnimation.SetBool("triggered by player", true);
        boxCollider.isTrigger = true;
        rb.isKinematic = false;
        audioSource.Stop();
        isFalling = true;
        StartCoroutine(WaitForPlatformUp(waitForSecondsUp));
    }

    private IEnumerator WaitForPlatformUp(float time)
    {
        yield return new WaitForSeconds(time);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(motorAudio, volume);
        }
        fallingAnimation.SetBool("triggered by player", false);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        isFalling = false;
        boxCollider.isTrigger = false;
        platformMovingBack = true;

    }
}
