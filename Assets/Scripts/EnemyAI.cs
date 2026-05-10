using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float speed = 2f;
    public float changeDirectionTime = 1.5f;
    public float detectionRange = 4f;

    private Vector2 moveDirection;
    private float timer;

    private Pathfinding pathfinding;
    private List<Node> path;
    private int targetIndex;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        pathfinding = FindObjectOfType<Pathfinding>();

        PickNewDirection();

        InvokeRepeating("UpdatePath", 0f, 0.3f);
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= detectionRange)
            {
                FollowPath();
                return;
            }
        }

        Wander();
    }

    void Wander()
    {
        Vector2 nextPos = rb.position + moveDirection * speed * Time.deltaTime;
        rb.MovePosition(nextPos);

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PickNewDirection();
        }
    }

    void UpdatePath()
    {
        if (player == null || pathfinding == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > detectionRange)
            return;

        List<Node> newPath = pathfinding.FindPath(transform.position, player.position);
        if (newPath != null && newPath.Count > 0)
        {
            path = newPath;
            targetIndex = 0;
        }
    }

    void FollowPath()
    {
        if (path == null || path.Count == 0)
            return;

        if (targetIndex >= path.Count)
            return;

        Vector2 targetPos = path[targetIndex].worldPosition;

        Vector2 nextPos = Vector2.MoveTowards(
            rb.position,
            targetPos,
            speed * Time.deltaTime
        );

        rb.MovePosition(nextPos);

        if (Vector2.Distance(rb.position, targetPos) < 0.05f)
        {
            targetIndex++;
        }
    }

    void PickNewDirection()
    {
        int r = Random.Range(0, 4);

        if (r == 0) moveDirection = Vector2.up;
        if (r == 1) moveDirection = Vector2.down;
        if (r == 2) moveDirection = Vector2.left;
        if (r == 3) moveDirection = Vector2.right;

        timer = changeDirectionTime;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Enemy kena: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Player"))
        {
            GameManager gm = FindObjectOfType<GameManager>();

            if (gm != null)
            {
                gm.LoseGame();
            }
        }
    }
}