using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    [Header("Vision Setting")]
    public float viewDistance = 6f;
    [Range(0, 180)]
    public float viewAngle = 90f;

    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    public Transform player;

    [HideInInspector]
    public bool canSeePlayer = false;

    Vector2 facingDirection = Vector2.right;

    void Start()
    {
        if (player == null)
        {
            GameObject p =
                GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
        }

        facingDirection = transform.right;
    }

    void Update()
    {
        DetectPlayer();
    }

    public void SetFacingDirection(Vector2 dir)
    {
        if (dir != Vector2.zero)
            facingDirection = dir.normalized;
    }

    void DetectPlayer()
    {
        canSeePlayer = false;

        if (player == null)
            return;

        Vector2 directionToPlayer =
            player.position - transform.position;

        //-----------------------------
        // 1. Cek Jarak
        //-----------------------------

        if (directionToPlayer.magnitude > viewDistance)
            return;

        //-----------------------------
        // 2. Cek Sudut
        //-----------------------------

        float angle =
            Vector2.Angle(facingDirection,
                          directionToPlayer);

        if (angle > viewAngle / 2)
            return;

        //-----------------------------
        // 3. Raycast
        //-----------------------------

        float distance = directionToPlayer.magnitude;

        Vector2 origin = transform.position;

        RaycastHit2D hit =
        Physics2D.Raycast(
        origin,
        directionToPlayer.normalized,
        distance,
        obstacleLayer | playerLayer);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                canSeePlayer = true;
            }
        }

        Debug.DrawRay(
        transform.position,
        directionToPlayer.normalized * directionToPlayer.magnitude,
        Color.red
);
    }

    public Vector2 GetFacingDirection()
    {
        return facingDirection;
    }
    void OnDrawGizmos()
    {
        Debug.Log("Draw Vision");

        Gizmos.color = canSeePlayer ? Color.red : Color.yellow;

        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 left =
            Quaternion.Euler(0, 0, -viewAngle / 2)
            * facingDirection;

        Vector3 right =
            Quaternion.Euler(0, 0, viewAngle / 2)
            * facingDirection;

        Gizmos.DrawLine(transform.position,
            transform.position + left * viewDistance);

        Gizmos.DrawLine(transform.position,
            transform.position + right * viewDistance);

        // gambar cone
        int step = 20;

        Vector3 lastPoint = transform.position + left * viewDistance;

        for (int i = 1; i <= step; i++)
        {
            float angle =
                -viewAngle / 2 +
                (viewAngle / step) * i;

            Vector3 dir =
                Quaternion.Euler(0, 0, angle)
                * facingDirection;

            Vector3 nextPoint =
                transform.position +
                dir * viewDistance;

            Gizmos.DrawLine(lastPoint, nextPoint);

            lastPoint = nextPoint;
        }
    }
}