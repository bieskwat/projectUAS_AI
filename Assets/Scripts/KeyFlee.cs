using UnityEngine;

public class KeyFlee : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float fleeDistance = 2f; // jarak trigger

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < fleeDistance)
        {
            Vector2 direction = (transform.position - player.position).normalized;
            rb.linearVelocity = direction * speed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}