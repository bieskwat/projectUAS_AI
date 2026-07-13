using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Investigate,
        Chase
    }

    [Header("Movement")]
    public float speed = 2f;

    [Header("Player")]
    public Transform player;

    [Header("Patrol")]
    private Node patrolTarget;
    private bool reachedPatrolTarget = true;

    private Vector3 investigatePosition;
    private float investigateTimer;
    public float investigateDuration = 3f;
    private EnemyState currentState = EnemyState.Patrol;

    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private Pathfinding pathfinding;
    private VisionSensor vision;

    private List<Node> path;
    private int targetIndex;


    private GridManager grid;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lineRenderer = GetComponent<LineRenderer>();

        pathfinding = FindFirstObjectByType<Pathfinding>();

        grid = FindFirstObjectByType<GridManager>();

        vision = GetComponent<VisionSensor>();

        if (lineRenderer == null)
            Debug.LogWarning("LineRenderer belum dipasang.");

        if (vision == null)
            Debug.LogError("VisionSensor belum dipasang.");

        currentState = EnemyState.Patrol;

        InvokeRepeating(nameof(UpdatePath), 0f, 0.3f);
    }

    void Update()
    {
        if (vision == null)
            return;

        if (vision.canSeePlayer)
        {
            currentState = EnemyState.Chase;
            return;
        }

        // kalo udh ga keliatan player, balik patrol
        if (currentState == EnemyState.Chase && !vision.canSeePlayer)
        {
            currentState = EnemyState.Patrol;
            path = null;
            reachedPatrolTarget = true;
        }

        if (SoundManager.soundMade)
        {
            float distance =
                Vector2.Distance(
                    transform.position,
                    SoundManager.soundPosition);

            if (distance <= SoundManager.soundRadius)
            {
                investigatePosition =
                    SoundManager.soundPosition;

                currentState =
                    EnemyState.Investigate;

                investigateTimer =
                    investigateDuration;
            }

            SoundManager.soundMade = false;
        }
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:

                PatrolUpdate();

                break;

            case EnemyState.Investigate:
                InvestigateUpdate();
                break;

            case EnemyState.Chase:

                FollowPath();

                break;

        }
    }

    //---------------------------------------------------
    // PATROL
    //---------------------------------------------------

    void PatrolUpdate()
    {
        if (reachedPatrolTarget || path == null)
        {
            patrolTarget = grid.GetRandomWalkableNode();

            while (patrolTarget == null)
            {
                patrolTarget = grid.GetRandomWalkableNode();
            }

            reachedPatrolTarget = false;

            UpdatePatrolPath();

            if (path == null)
            {
                reachedPatrolTarget = true;
                return;
            }
        }

        FollowPath();
    }

    void UpdatePatrolPath()
    {
        if (patrolTarget == null)
            return;

        List<Node> newPath =
            pathfinding.FindPath(
                transform.position,
                patrolTarget.worldPosition
            );

        if (newPath != null && newPath.Count > 0)
        {
            path = newPath;

            targetIndex = 1;

            if (targetIndex >= path.Count)
                targetIndex = 0;

            DrawPath();
        }
    }

    void InvestigateUpdate()
    {
        investigateTimer -= Time.fixedDeltaTime;

        if (path == null)
        {
            path = pathfinding.FindPath(
                transform.position,
                investigatePosition
            );

            targetIndex = 1;
        }

        FollowPath();

        if (investigateTimer <= 0)
        {
            currentState = EnemyState.Patrol;

            path = null;
        }
    }

    //---------------------------------------------------
    // CHASE
    //---------------------------------------------------

    void UpdatePath()
    {
        if (currentState != EnemyState.Chase)
            return;

        if (player == null || pathfinding == null)
            return;

        List<Node> newPath =
            pathfinding.FindPath(
                transform.position,
                player.position
            );

        if (newPath != null && newPath.Count > 0)
        {
            path = newPath;

            targetIndex = 1;

            if (targetIndex >= path.Count)
                targetIndex = 0;

            DrawPath();
        }
        else
        {
            if (lineRenderer != null)
                lineRenderer.positionCount = 0;
        }
    }

    void FollowPath()
    {
        if (path == null || path.Count == 0)
            return;

        if (targetIndex >= path.Count)
            return;

        Vector2 targetPos =
            path[targetIndex].worldPosition;

        Vector2 dir =
            (targetPos - rb.position).normalized;

        vision.SetFacingDirection(dir);

        // Flip sprite sesuai arah gerak
        //if (dir.x > 0.05f)
        //{
        //    transform.localScale = new Vector3(1, 1, 1);
        //}
        //else if (dir.x < -0.05f)
        //{
        //    transform.localScale = new Vector3(-1, 1, 1);
        //}

        Vector2 nextPos =
            Vector2.MoveTowards(
                rb.position,
                targetPos,
                speed * Time.fixedDeltaTime
            );

        rb.MovePosition(nextPos);

        if (Vector2.Distance(rb.position, targetPos) < 0.05f)
        {
            targetIndex++;
            if (targetIndex >= path.Count)
            {
                if (currentState == EnemyState.Patrol)
                {
                    reachedPatrolTarget = true;
                    path = null;
                }

                return;
            }
        }
    }

    //---------------------------------------------------
    // DRAW PATH
    //---------------------------------------------------

    void DrawPath()
    {
        if (lineRenderer == null)
            return;

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

    //---------------------------------------------------
    // PLAYER HIT
    //---------------------------------------------------

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager gm =
                FindFirstObjectByType<GameManager>();

            if (gm != null)
            {
                gm.LoseGame();
            }
        }
    }
}