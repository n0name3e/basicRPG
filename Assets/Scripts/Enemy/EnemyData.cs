using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public float health;
    public float speed;
    public float attackDelay;
    public float attackCooldown;
    public float detectionRange;
    public float attackRange; // distance at which enemy starts attacking
    public float attackDistance; // distance at which the attack would hit

    public GameObject prefab;
}
