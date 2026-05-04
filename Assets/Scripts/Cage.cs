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
        if (other.CompareTag("Player"))
        {
            if (isUnlocked)
            {
                Debug.Log("Anak diselamatkan!");
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Butuh kunci!");
            }
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