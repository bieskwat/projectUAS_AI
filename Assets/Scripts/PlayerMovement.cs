using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Ambil input keyboard
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalisasi supaya tidak lebih cepat diagonal
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}