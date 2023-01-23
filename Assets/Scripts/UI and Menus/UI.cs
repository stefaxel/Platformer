using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cherryText;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject cherryCounter;

    PlayerHealth playerHealth;

    public int numOfCherries { get; private set; }

    public bool respawnPressed { get; private set; }

    private void Start()
    {
        numOfCherries = 0;
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

    public void OnClickRespawn()
    {
        respawnPressed = true;
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameOverScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gameOverScreen.SetActive(false);
        cherryCounter.SetActive(true);
        respawnPressed = false;
        numOfCherries = numOfCherries - 5;
        cherryText.text = "Cherry: " + numOfCherries.ToString();
    }
}
