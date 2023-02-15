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
    [SerializeField] GameObject volumeUI;

    public static bool isPaused = false;
    private bool volumeWIndowOpen = false;

    [SerializeField] private CherryScoreSO scoreSO;

    PlayerHealth playerHealth;

    PauseAction action;
    InputAction ui;

    private float timer;

    public int numOfCherries { get; private set; }

    public bool respawnPressed { get; private set; }

    private void Start()
    {
        numOfCherries = scoreSO.Value;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        cherryText.text = "Cherry: " + numOfCherries.ToString();

        action.Pause.PauseGame.performed += _ => DeterminePause();
    }

    private void DeterminePause()
    {
        if (isPaused && !volumeWIndowOpen)
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
        scoreSO.Value = numOfCherries;
        cherryText.text = "Cherry: " + scoreSO.Value.ToString();
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
        scoreSO.Value = numOfCherries;
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
        numOfCherries = 0;
        scoreSO.Value = numOfCherries;
        timer = 0;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void VolumeButton()
    {
        volumeUI.SetActive(true);
        volumeWIndowOpen = true;
    }

    public void VolumeCloseButton()
    {
        volumeWIndowOpen = false;
        volumeUI.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
