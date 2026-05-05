using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceneloader : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadWin()
    {
        SceneManager.LoadScene("WinScene");
    }

    public void LoadLose()
    {
        SceneManager.LoadScene("LoseScene");
    }
}