using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsMenu : MonoBehaviour
{
    public void OnClickStart()
    {
        Invoke("OnClickStartGameEffect", 0.25f);
    }

    private void OnClickStartGameEffect()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
