using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    private int finalScore;

    [SerializeField] private CherryScoreSO scoreSO;

    [SerializeField] private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        finalScore = scoreSO.Value;
        scoreText.text = "Score: " + finalScore.ToString();
    }

    public void OnClickMainMenu()
    {
        Invoke("OnClickMainMenuEffect", 0.25f);
    }
    
    private void OnClickMainMenuEffect()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickQuit()
    {
        Invoke("OnClickQuitEffect", 0.25f);
    }

    private void OnClickQuitEffect()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}
