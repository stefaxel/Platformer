using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject volumeMenu;

    public void OnClickStartGame()
    {
        SceneManager.LoadScene(3);
    }

    public void OnClickControls()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickVolume()
    {
        volumeMenu.SetActive(true);
    }

    public void OnClickBack()
    {
        volumeMenu.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
