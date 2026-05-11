using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    public float speed = 2f;
    public float changeDirectionTime = 2f;

    private Vector2 moveDirection;
    private float timer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        PickNewDirection();
    }

    void FixedUpdate()
    {
        Vector2 nextPos = rb.position + moveDirection * speed * Time.fixedDeltaTime;

        rb.MovePosition(nextPos);

        timer -= Time.fixedDeltaTime;

        if (timer <= 0f)
        {
            PickNewDirection();
        }
    }

    void PickNewDirection()
    {
        int random = Random.Range(0, 4);

        if (random == 0)
            moveDirection = Vector2.up;

        if (random == 1)
            moveDirection = Vector2.down;

        if (random == 2)
            moveDirection = Vector2.left;

        if (random == 3)
            moveDirection = Vector2.right;

        timer = changeDirectionTime;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
       
        if (other.gameObject.CompareTag("Wall"))
        {
            PickNewDirection();
        }

        if (other.gameObject.CompareTag("Player"))
        {
            GameManager gm = FindFirstObjectByType<GameManager>();

            if (gm != null)
            {
                gm.LoseGame();
            }
        }
    }
}