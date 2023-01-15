using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FanTrap : MonoBehaviour
{
    [SerializeField] float onTime;
    [SerializeField] float offTime;
    [SerializeField] float forceApplied;
    [SerializeField] bool vertical;
    [SerializeField] bool horizontal;

    [SerializeField] bool down;
    [SerializeField] bool left;
    [SerializeField] bool right;
    [SerializeField] bool up;
    [SerializeField] bool angle;

    int forceDirection;
    Vector3 applyForceUp;
    Vector3 applyForceAcross;
    Vector3 applyForceUpAcross;

    bool fanIsActive = true;

    float rotation;
    
    private bool isActive = true;
    private bool fanCanBlowPlayer;

    private void Start()
    {
        StartCoroutine(FanTrapTrigger());
        rotation = gameObject.transform.localRotation.z;

        if (down)
            forceDirection = -1;
        if (left)
            forceDirection = -1;
        if (right)
            forceDirection = 1;
        if (up)
            forceDirection = 1;

        applyForceUp = new Vector3(0, forceDirection, rotation);
        applyForceAcross = new Vector3(forceDirection, 0, rotation);
        applyForceUpAcross = new Vector3(forceDirection, forceDirection, rotation);
    }

    private void Update()
    {
        if(!fanIsActive)
            StartCoroutine(FanTrapTrigger());
    }

    IEnumerator FanTrapTrigger()
    {
        if (!isActive)
        {
            fanCanBlowPlayer = false;
            yield return new WaitForSeconds(offTime);
            fanIsActive = true;
            isActive = true;
        }

        if (isActive)
        {
            fanCanBlowPlayer = true;
            yield return new WaitForSeconds(onTime);
            isActive = false;
            fanIsActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (isActive && fanCanBlowPlayer && vertical)
            {
                collision.GetComponent<Rigidbody2D>().AddRelativeForce(applyForceUp * forceApplied, ForceMode2D.Impulse);
            }

            if (isActive && fanCanBlowPlayer && horizontal)
            {
                collision.GetComponent<Rigidbody2D>().AddRelativeForce(applyForceAcross * forceApplied, ForceMode2D.Impulse);
            }

            if (isActive && fanCanBlowPlayer && angle)
            {
                collision.GetComponent<Rigidbody2D>().AddRelativeForce(applyForceUpAcross * forceApplied, ForceMode2D.Impulse);
            }
        }
    }

}
