using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyData enemyToSpawn;

    private void Start()
    {
        GameObject enemyObject = Instantiate(enemyToSpawn.prefab, transform.position, Quaternion.identity);

        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemy.maxHealth = enemyToSpawn.health;
        enemy.Health = enemyToSpawn.health;
        enemy.speed = enemyToSpawn.speed;
        enemy.attackDelay = enemyToSpawn.attackDelay;
        enemy.attackCooldown = enemyToSpawn.attackCooldown;
        enemy.detectionRange = enemyToSpawn.detectionRange;
        enemy.attackRange = enemyToSpawn.attackRange;
        enemy.attackDistance = enemyToSpawn.attackDistance;
    }
}
