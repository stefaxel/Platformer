using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanTrap : MonoBehaviour
{
    [SerializeField] float onTime;
    [SerializeField] float offTime;
    [SerializeField] float forceApplied;

    private bool isActive;
    private bool canBeBlown;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FanTrapTrigger());
    }

    IEnumerator FanTrapTrigger()
    {
        if (!isActive)
        {
            canBeBlown = false;
            isActive = true;
            yield return new WaitForSeconds(onTime);
        }

        if (isActive)
        {
            canBeBlown = true;
            yield return new WaitForSeconds(offTime);
            isActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (isActive && canBeBlown)
            {
                collision.GetComponent<Rigidbody2D>().AddForce(Vector2.up * forceApplied, ForceMode2D.Impulse);
            }
        }
    }

}
