using UnityEngine;

public class EnemyPatrolling : MonoBehaviour
{
    [SerializeField] private Vector2 startingDestination;
    [SerializeField] private Vector2 endingDestination;
    private Enemy enemy;
    private Vector2 direction;
    private Vector2 oppositeDirection;
    private float speed;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        speed = enemy.speed;
        direction = startingDestination;
        oppositeDirection = endingDestination;
    }

    public void Patrol()
    {
        //if (enemy.State != EnemyState.Patrolling) return;
        transform.position = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);
        //transform.position = Vector2.Lerp(transform.position, direction, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, direction) <= 0.2f)
        {
            Vector2 vector2 = new Vector2(direction.x, direction.y);
            direction = oppositeDirection;
            oppositeDirection = vector2;
        }
    }
}
