using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;

    Pathfinding pathfinding;
    List<Node> path;
    int targetIndex;

    void Start()
    {
        pathfinding = FindObjectOfType<Pathfinding>();
        InvokeRepeating("UpdatePath", 0, 1f);
    }

    void UpdatePath()
    {
        path = pathfinding.FindPath(transform.position, player.position);
        targetIndex = 0;
    }

    void Update()
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
}