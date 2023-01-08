using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float waitForSecondsDown;
    [SerializeField] float waitForSecondsUp;
    [SerializeField] float speedUp;
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    private Vector2 initialPos;
    private Vector2 currentPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        initialPos = transform.position;
        if (currentPosition == initialPos)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Update()
    {
        currentPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            StartCoroutine(WaitForPlatformFall(waitForSecondsDown));
        }
    }

    private IEnumerator WaitForPlatformFall(float time)
    {
        yield return new WaitForSeconds(time);
    
        boxCollider.isTrigger = true;
        rb.isKinematic = false;
        StartCoroutine(WaitForPlatformUp(waitForSecondsUp));
    }

    private IEnumerator WaitForPlatformUp(float time)
    {
        yield return new WaitForSeconds(time);

        rb.isKinematic = true;
        boxCollider.isTrigger = false;

        if(currentPosition != initialPos)
        {
            rb.velocity = Vector2.up * speedUp;
            
        }
        
        //transform.position = initialPos;
        //rb.velocity = Vector2.zero;
    }
}
