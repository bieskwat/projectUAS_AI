using UnityEngine;

public class Cage : MonoBehaviour
{
    public bool isUnlocked = false;
    private Animator animator;

    void Update()
    {
        if (!isUnlocked && KeyPickup.hasKey)
        {
            Unlock();
        }
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger rumah kena: " + other.name);

        if (other.CompareTag("Player") && isUnlocked)
        {
            GameManager gm = FindFirstObjectByType<GameManager>();

            if (gm != null)
            {
                gm.WinGame();
            }
        }
    }
    public void Unlock()
    {
        isUnlocked = true;

        if (animator != null)
        {
            animator.SetTrigger("Open");
        }

        Debug.Log("Cage terbuka!");
    }
}