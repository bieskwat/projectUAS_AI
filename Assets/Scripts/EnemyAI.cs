using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;

    private Pathfinding pathfinding;
    private List<Node> path;
    private int targetIndex;

    private Rigidbody2D rb;
    private LineRenderer lineRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lineRenderer = GetComponent<LineRenderer>();

        pathfinding = FindObjectOfType<Pathfinding>();

        InvokeRepeating("UpdatePath", 0f, 0.3f);
    }

    void FixedUpdate()
    {
        FollowPath();
    }

    void UpdatePath()
    {
        if (player == null || pathfinding == null)
            return;

        List<Node> newPath = pathfinding.FindPath(transform.position, player.position);

        if (newPath != null && newPath.Count > 0)
        {
            path = newPath;

            targetIndex = 1;

            if (targetIndex >= path.Count)
            {
                targetIndex = 0;
            }

            DrawPath();
        }
        else
        {
            lineRenderer.positionCount = 0;
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
            speed * Time.fixedDeltaTime
        );

        rb.MovePosition(nextPos);

        if (Vector2.Distance(rb.position, targetPos) < 0.05f)
        {
            targetIndex++;
        }
    }

    void DrawPath()
    {
        if (path == null || path.Count == 0)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = path.Count + 1;

        lineRenderer.SetPosition(0, transform.position);

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 pos = new Vector3(
                path[i].worldPosition.x,
                path[i].worldPosition.y,
                -1f
            );

            lineRenderer.SetPosition(i + 1, pos);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
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