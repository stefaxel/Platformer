using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject volumeMenu;

    public void OnClickStartGame()
    {
        Invoke("OnClickStartEffect", 0.25f);
    }

    private void OnClickStartEffect()
    {
        SceneManager.LoadScene(2);
    }

    public void OnClickControls()
    {
        Invoke("OnClickControlsEffect", 0.25f);
    }

    private void OnClickControlsEffect()
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
        Invoke("BackToMainMenuEffect", 0.25f);
    }

    private void BackToMainMenuEffect()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Invoke("QuitGameEffect", 0.25f);
    }

    private void QuitGameEffect()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}

