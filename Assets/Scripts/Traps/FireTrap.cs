using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] float onTime;
    [SerializeField] float offTime;
    [SerializeField] int damage;

    private bool isActive;
    private bool canTakeDamage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FireTrapTrigger());
    }

    private IEnumerator FireTrapTrigger()
    {
        float startTime = Time.time;

        if(!isActive)
        {
            canTakeDamage = false;
            isActive = true;
            yield return new WaitForSeconds(onTime);
        }

        if(isActive)
        {
            canTakeDamage = true;
            yield return new WaitForSeconds(offTime);
            isActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            if (isActive && canTakeDamage)
            {
                collision.GetComponent<PlayerHealth>().TakeDamage(damage);
                Debug.Log("Damage taken = " + damage);
            }
        }
    }
}
