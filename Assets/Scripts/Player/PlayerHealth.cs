using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] public int currentHealth { get; private set; }

    [SerializeField] GameObject gameOverScreen;
    bool restartPressed = false;

    [SerializeField] private Healthbar healthbar;

    PlayerPosition playerPosition;

    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPosition>();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth); 
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("Do something");
        }
    }

    public void OnClick()
    {
        restartPressed = true;
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        if (restartPressed)
        {
            currentHealth = maxHealth;
            Time.timeScale = 1;
            transform.position = playerPosition.respawnPlayer;
            gameOverScreen.SetActive(false);
            restartPressed = false;
        }
    }

}
