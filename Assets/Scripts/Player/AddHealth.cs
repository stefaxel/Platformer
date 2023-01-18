using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealth : MonoBehaviour
{
    [SerializeField] int healthPoint;

    [SerializeField] PlayerHealth playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            if(playerHealth.currentHealth < 5)
            {
                playerHealth.AddHealth(healthPoint);
                Destroy(gameObject);
            }
        }
    }
}
