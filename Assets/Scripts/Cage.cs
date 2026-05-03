using UnityEngine;

public class Cage : MonoBehaviour
{
    public bool isUnlocked = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isUnlocked)
        {
            Debug.Log("Anak diselamatkan!");
            Destroy(gameObject);
        }
    }

    public void Unlock()
    {
        isUnlocked = true;
        Debug.Log("Cage terbuka!");
    }
}