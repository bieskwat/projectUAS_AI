using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public static bool hasKey = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hasKey = true;
            Debug.Log("Key diambil!");
            Destroy(gameObject);
        }
    }
}