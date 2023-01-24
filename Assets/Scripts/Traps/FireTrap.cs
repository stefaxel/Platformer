using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : DamageScript
{
    [SerializeField] float onTime;
    [SerializeField] float offTime;
    //[SerializeField] int damage;

    private bool isActive;
    private bool fireIsActive = false;
    private bool canTakeDamage;

    private Animator fireAnimation;

    void Start()
    {
        fireAnimation = GetComponent<Animator>();
        StartCoroutine(FireTrapTrigger());
    }

    // Update is called once per frame
    void Update()
    {
        if(!fireIsActive)
            StartCoroutine(FireTrapTrigger());
    }

    private IEnumerator FireTrapTrigger()
    { 
        if(!isActive)
        {
            fireAnimation.SetBool("fire active", false);
            canTakeDamage = false;
            yield return new WaitForSeconds(offTime);
            fireIsActive = true;
            isActive = true;
        }

        if(isActive)
        {
            fireAnimation.SetBool("fire active", true);
            canTakeDamage = true;
            yield return new WaitForSeconds(onTime);
            isActive = false;
            fireIsActive = false;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
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
