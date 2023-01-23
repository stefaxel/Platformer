using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] public int currentHealth { get; private set; }

    [SerializeField] private Healthbar healthbar;
    PlayerPosition playerPosition;
    UI ui;

    private bool canTakeDamage = true;
    [SerializeField] private float damageCooldown;
    
    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPosition>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth); 
    }

    private void Update()
    {
        RespawnPlayer();
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            currentHealth -= damage;
            healthbar.SetHealth(currentHealth);
            canTakeDamage = false;
            StartCoroutine(DamageCooldown(damageCooldown));
        }
    }

    private void RespawnPlayer()
    {
        if (ui.respawnPressed && ui.numOfCherries == 5)
        {
            ui.RestartGame();
            currentHealth = maxHealth;
            healthbar.SetHealth(currentHealth);
            transform.position = playerPosition.respawnPlayer;
        }
    }

    public void AddHealth(int health)
    {
        currentHealth = currentHealth + health;
        healthbar.SetHealth(currentHealth);
    }

    private IEnumerator DamageCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canTakeDamage = true;
    }

}
