using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //public static event Action OnPlayerDeath;

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
        OnDeath();
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

    public void OnDeath()
    {
        //bool triggerDeath = true;

        if (currentHealth <= 0)
        {
            canTakeDamage = false;
            rb.bodyType = RigidbodyType2D.Static;

            //ui.PlayerHealth();

            //PlayerDeath();


            playerDeath.SetBool("death", true);
            //playerDeath.SetTrigger("death");

            //if (triggerDeath)
            //{
            //    playerDeath.SetTrigger("death");
            //    triggerDeath = false;
            //    Debug.Log(triggerDeath);
            //}


            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //SoundManager.instance.PlaySound(failSound);
        }
    }

    public void RestartLevel()
    {
        ui.PlayerHealth();
    }

    //public void PlayerDeath()
    //{
    //    OnPlayerDeath?.Invoke();
    //}

}
