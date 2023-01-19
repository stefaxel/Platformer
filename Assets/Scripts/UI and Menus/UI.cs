using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cherryText;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject cherryCounter;

    PlayerHealth playerHealth;

    int numOfCherries = 0;

    public bool restartPressed { get; private set; }

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        cherryText.text = "Cherry: " + numOfCherries.ToString(); 
    }

    private void Update()
    {
        PlayerHealth();
    }

    public void AddCollectable(int collectable)
    {
        numOfCherries = numOfCherries + collectable;
        cherryText.text = "Cherry: " + numOfCherries.ToString();
    }

    private void PlayerHealth()
    {
        if(playerHealth.currentHealth <= 0)
        {
            cherryCounter.SetActive(false);
            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void OnClick()
    {
        restartPressed = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gameOverScreen.SetActive(false);
        cherryCounter.SetActive(true);
        restartPressed = false;
    }
}
