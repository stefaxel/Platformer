using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cherryText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject cherryCounter;
    [SerializeField] GameObject timeCounter;

    PlayerHealth playerHealth;

    private float timer;

    public int numOfCherries { get; private set; }

    public bool respawnPressed { get; private set; }
    

    private void Start()
    {
        numOfCherries = 0;
        //timer = 0;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        cherryText.text = "Cherry: " + numOfCherries.ToString(); 
    }

    private void Update()
    {
        PlayerHealth();
        LevelTime();
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
            timeCounter.SetActive(false);
            gameOverScreen.SetActive(true);   
        }
    }

    public void OnClickRespawn()
    {
        Time.timeScale = 0;
        respawnPressed = true;
    }

    public void OnClickRestart()
    {
        RestartLevel();
    }

    private void LevelTime()
    {
        timer += Time.deltaTime;
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        timeText.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gameOverScreen.SetActive(false);
        cherryCounter.SetActive(true);
        timeCounter.SetActive(true);
        respawnPressed = false;
        numOfCherries = numOfCherries - 5;
        cherryText.text = "Cherry: " + numOfCherries.ToString();
    }

    private void RestartLevel()
    {
        timer = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
