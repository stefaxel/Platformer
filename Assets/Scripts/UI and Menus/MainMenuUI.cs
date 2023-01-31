using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
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
        SceneManager.LoadScene(2);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
