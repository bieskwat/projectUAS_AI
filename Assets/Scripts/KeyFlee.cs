using UnityEngine;

public class KeyFlee : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float fleeDistance = 3f;

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < fleeDistance)
        {
            Vector2 dir = (transform.position - player.position).normalized;
            transform.position += (Vector3)dir * speed * Time.deltaTime;
        }
    }
}