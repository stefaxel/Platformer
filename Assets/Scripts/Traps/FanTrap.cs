using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FanTrap : MonoBehaviour
{
    [SerializeField] float onTime;
    [SerializeField] float offTime;
    [SerializeField] float forceApplied;
    [SerializeField] bool up;
    [SerializeField] bool down;
    [SerializeField] bool left;
    [SerializeField] bool right;

    bool fanIsActive = true;

    float rotation;
    
    private bool isActive = true;
    private bool canBeBlown;

    private void Start()
    {
        StartCoroutine(FanTrapTrigger());
        rotation = gameObject.transform.localRotation.z;
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
            canBeBlown = false;
            yield return new WaitForSeconds(offTime);
            fanIsActive = true;
            isActive = true;
        }

        if (isActive)
        {
            canBeBlown = true;
            yield return new WaitForSeconds(onTime);
            isActive = false;
            fanIsActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 applyForceDir = new Vector3(0, 0, rotation);
        if (collision.gameObject.name == "Player")
        {
            if (isActive && canBeBlown && up)
            {
                Debug.Log(applyForceDir);
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * forceApplied, ForceMode2D.Impulse);
            }

            if (isActive && canBeBlown && down)
            {
                Debug.Log(applyForceDir);
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.down * forceApplied, ForceMode2D.Impulse);
            }

            if (isActive && canBeBlown && left)
            {
                Debug.Log(applyForceDir);
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.left * forceApplied, ForceMode2D.Impulse);
            }

            if (isActive && canBeBlown && right)
            {
                Debug.Log(applyForceDir);
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.right * forceApplied, ForceMode2D.Impulse);
            }
        }
    }

}
