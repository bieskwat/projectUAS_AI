using UnityEngine;

public class Cage : MonoBehaviour
{
    public bool isUnlocked = false;

    void Update()
    {
        // cek apakah player sudah punya key
        if (KeyPickup.hasKey)
        {
            Unlock();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger kena: " + other.name);
        if (other.CompareTag("Player") && isUnlocked)
        {
            FindObjectOfType<GameManager>().WinGame();
            Destroy(gameObject);
        }
    }

    public void Unlock()
    {
        if (!isUnlocked)
        {
            isUnlocked = true;
            Debug.Log("Cage terbuka!");
        }
    }
}