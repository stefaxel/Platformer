using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int health;
    [HideInInspector] public int currentHealth { get; private set; }

    [SerializeField] GameObject gameOverScreen;
    bool restartPressed = false;

    PlayerPosition playerPosition;

    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPosition>();
        currentHealth = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
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
            health = currentHealth;
            Time.timeScale = 1;
            transform.position = playerPosition.respawnPlayer;
            gameOverScreen.SetActive(false);
            restartPressed = false;
        }
    }

}
