using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float waitForSecondsDown;
    [SerializeField] float waitForSecondsUp;
    [SerializeField] float speed;

    Vector2 initialPos;
    BoxCollider2D boxCollider;
    Rigidbody2D rb;

    bool platformMovingBack;

    private Animator fallingAnimation;

    void Awake()
    {
        fallingAnimation = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        initialPos = transform.position;
    }

    private void Update()
    {
        if (platformMovingBack)
            transform.position = Vector2.MoveTowards(transform.position, initialPos, speed * Time.deltaTime);

        if (transform.position.y == initialPos.y)
            platformMovingBack = false;
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
        StartCoroutine(WaitForPlatformUp(waitForSecondsUp));
    }

    private IEnumerator WaitForPlatformUp(float time)
    {
        yield return new WaitForSeconds(time);

        fallingAnimation.SetBool("triggered by player", false);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        boxCollider.isTrigger = false;
        platformMovingBack = true;

    }
}
