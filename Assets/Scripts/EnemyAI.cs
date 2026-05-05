using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Idle,
        Wander,
        Chase
    }

    public State currentState;

    public Transform player;
    public float speed = 3f;
    public float detectionRange = 5f;

    private Vector2 wanderDirection;

    private Pathfinding pathfinding;
    private List<Node> path;
    private int targetIndex;

    private LineRenderer lineRenderer;

    void Start()
    {
        currentState = State.Wander;

        pathfinding = FindObjectOfType<Pathfinding>();

        InvokeRepeating("ChangeDirection", 0, 2f);
        InvokeRepeating("UpdatePath", 0, 1f); // update path tiap 1 detik
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                IdleState(distance);
                break;

            case State.Wander:
                WanderState(distance);
                break;

            case State.Chase:
                ChaseState(distance);
                break;
        }
    }

    // ================= FSM =================

    void IdleState(float distance)
    {
        if (distance < detectionRange)
            currentState = State.Chase;
    }

    void WanderState(float distance)
    {
        transform.position += (Vector3)wanderDirection * speed * Time.deltaTime;

        if (distance < detectionRange)
            currentState = State.Chase;
    }

    void ChaseState(float distance)
    {
        FollowPath();

        if (distance > detectionRange)
            currentState = State.Wander;
    }

    // ================= PATHFINDING =================

    void UpdatePath()
    {
        if (currentState == State.Chase)
        {
            path = pathfinding.FindPath(transform.position, player.position);
            targetIndex = 0;

            DrawPath();
        }
    }

    void FollowPath()
    {
        if (path == null || path.Count == 0)
            return;

        Vector2 targetPos = path[targetIndex].worldPosition;
        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;

        transform.position += (Vector3)dir * speed * Time.deltaTime;

        if (Vector2.Distance(transform.position, targetPos) < 0.2f)
        {
            targetIndex++;

            if (targetIndex >= path.Count)
                targetIndex = path.Count - 1;
        }
    }

    // ================= WANDER =================

    void ChangeDirection()
    {
        wanderDirection = Random.insideUnitCircle.normalized;
    }

    void DrawPath()
    {
        if (path == null)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 pos = path[i].worldPosition;
            pos.z = -1; // supaya terlihat di depan
            lineRenderer.SetPosition(i, pos);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().LoseGame();
        }
    }
}