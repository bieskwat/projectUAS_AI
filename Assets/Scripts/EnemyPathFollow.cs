using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;

    Pathfinding pathfinding;
    List<Node> path;
    int targetIndex;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pathfinding = FindObjectOfType<Pathfinding>();
        InvokeRepeating("UpdatePath", 0, 1f);
    }

    void UpdatePath()
    {
        path = pathfinding.FindPath(transform.position, player.position);
        targetIndex = 0;
    }

    void FixedUpdate()
    {
        if (path == null || path.Count == 0)
            return;

        Vector2 targetPos = path[targetIndex].worldPosition;

        Vector2 nextPos = Vector2.MoveTowards(
            rb.position,
            targetPos,
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(nextPos);

        if (Vector2.Distance(rb.position, targetPos) < 0.2f)
        {
            targetIndex++;

            if (targetIndex >= path.Count)
                targetIndex = path.Count - 1;
        }
    }
}