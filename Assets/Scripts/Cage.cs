using UnityEngine;

public class Cage : MonoBehaviour
{
    public bool isUnlocked = false;

    void Update()
    {
        if (!isUnlocked && KeyPickup.hasKey)
        {
            Unlock();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger rumah kena: " + other.name);

        if (other.CompareTag("Player") && isUnlocked)
        {
            GameManager gm = FindObjectOfType<GameManager>();

            if (gm != null)
            {
                gm.WinGame();
            }
        }
    }

    public void Unlock()
    {
        isUnlocked = true;
        Debug.Log("Cage terbuka!");
    }
}