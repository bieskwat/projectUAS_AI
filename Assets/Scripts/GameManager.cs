using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameEnded = false;

    public void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("WIN DIPANGGIL");
        SceneManager.LoadScene("WinScene");
    }

    public void LoseGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("LOSE DIPANGGIL");

        SceneManager.LoadScene("LoseScene");
    }
}