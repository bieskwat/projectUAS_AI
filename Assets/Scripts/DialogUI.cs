using UnityEngine;
using TMPro;

public class DialogUI : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text dialogText;

    public void ShowDialog(string message)
    {
        panel.SetActive(true);
        dialogText.text = message;
        Time.timeScale = 0f;
    }

    public void CloseDialog()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }
}