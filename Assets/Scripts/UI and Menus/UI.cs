using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;
using System;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cherryText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject cherryCounter;
    [SerializeField] GameObject timeCounter;
    [SerializeField] GameObject pauseUI;

    public static bool isPaused = false;

    PlayerHealth playerHealth;

    PauseAction action;
    InputAction ui;

    private float timer;

    public int numOfCherries { get; private set; }

    public bool respawnPressed { get; private set; }

    private void Start()
    {
        numOfCherries = 0;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        cherryText.text = "Cherry: " + numOfCherries.ToString();

        action.Pause.PauseGame.performed += _ => DeterminePause();
    }

    private void DeterminePause()
    {
        if (isPaused)
            DeactivateMenu();
        else
            ActivateMenu();
    }

    private void Awake()
    {
        action = new PauseAction();
    }

    private void Update()
    {
        //PlayerHealth();
        LevelTime();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseUI.SetActive(false);

        isPaused = false;
    }

    public void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseUI.SetActive(true);

        isPaused = true;
    }

    public void AddCollectable(int collectable)
    {
        numOfCherries = numOfCherries + collectable;
        cherryText.text = "Cherry: " + numOfCherries.ToString();
    }

    public void PlayerHealth()
    {

        isPaused = true;
        cherryCounter.SetActive(false);
        timeCounter.SetActive(false);
        gameOverScreen.SetActive(true);

    }

    public void OnClickRespawn()
    {
        if (numOfCherries >= 5)
        {
            Time.timeScale = 0;
            respawnPressed = true;
        }
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
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        gameOverScreen.SetActive(false);
        cherryCounter.SetActive(true);
        timeCounter.SetActive(true);
        respawnPressed = false;
        numOfCherries = numOfCherries - 5;
        isPaused = false;
        cherryText.text = "Cherry: " + numOfCherries.ToString();
    }

    public void RestartGameButton()
    {
        pauseUI.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1;
        RestartLevel();
    }

    public void ToMainMenu()
    {
        pauseUI.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1;
        isPaused = false;

        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        timer = 0;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
