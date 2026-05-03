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

    void Start()
    {
        currentState = State.Wander;
        InvokeRepeating("ChangeDirection", 0, 2f);
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

    void IdleState(float distance)
    {
        if (distance < detectionRange)
        {
            currentState = State.Chase;
        }
    }

    void WanderState(float distance)
    {
        transform.position += (Vector3)wanderDirection * speed * Time.deltaTime;

        if (distance < detectionRange)
        {
            currentState = State.Chase;
        }
    }

    void ChaseState(float distance)
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)dir * speed * Time.deltaTime;

        if (distance > detectionRange)
        {
            currentState = State.Wander;
        }
    }

    void ChangeDirection()
    {
        wanderDirection = Random.insideUnitCircle.normalized;
    }
}