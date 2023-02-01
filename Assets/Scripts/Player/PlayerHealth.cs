using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] public int currentHealth { get; private set; }

    [SerializeField] private Healthbar healthbar;
    PlayerPosition playerPosition;
    UI ui;

    private bool canTakeDamage = true;
    [SerializeField] private float damageCooldown;

    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip failSound;

    private Animator playerDeath;

    bool deathAnimation = true;

    private Rigidbody2D rb;

    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPosition>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
        
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        
        playerDeath = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        RespawnPlayer();

        if(deathAnimation)
            OnDeath();

        Debug.Log(deathAnimation);
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            currentHealth -= damage;
            if(currentHealth > 0)
                SoundManager.instance.PlaySound(hurtSound);
            healthbar.SetHealth(currentHealth);
            canTakeDamage = false;
            StartCoroutine(DamageCooldown(damageCooldown));
        }
    }

    public void RespawnPlayer()
    {
        if (ui.respawnPressed && ui.numOfCherries >= 5)
        {
            deathAnimation = true;

            ui.RestartGame();
            currentHealth = maxHealth;
            healthbar.SetHealth(currentHealth);

            rb.bodyType = RigidbodyType2D.Dynamic;

            transform.position = playerPosition.respawnPlayer;
            
            playerDeath.SetBool("death", false);
            canTakeDamage = true;
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

    private void OnDeath()
    {
        bool triggerDeath = true;
        if (currentHealth <= 0 && deathAnimation)
        {
            
            canTakeDamage = false;
            rb.bodyType = RigidbodyType2D.Static;


            playerDeath.SetBool("death", true);
            if(triggerDeath)
            {
                deathAnimation = false;
            }
            
            
        }

        triggerDeath = false;

    }

    public void RestartLevel()
    {
        deathAnimation = true;
        ui.PlayerHealth();
    }

    

}
