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
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);
    }

    private void RespawnPlayer()
    {
        if (ui.restartPressed)
        {
            ui.RestartGame();
            currentHealth = maxHealth;
            transform.position = playerPosition.respawnPlayer;
        }
    }

    public void AddHealth(int health)
    {
        currentHealth = currentHealth + health;
        healthbar.SetHealth(currentHealth);
    }

}
