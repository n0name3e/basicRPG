using UnityEngine;

enum AttackState
{
    None,
    Hitting,
    Cooldowning
}
public class EnemyAttacking : MonoBehaviour
{
    private Enemy enemy;
    private Player player;
    private float attackTimer = 0.0f;
    private AttackState attackState = AttackState.None;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        if (enemy.state == EnemyState.Attacking) attackTimer += Time.deltaTime;
        Chase();
        Attack();
    }
    private void Chase()
    {
        if (enemy.state != EnemyState.Chasing) return;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.speed * Time.deltaTime);
    }
    private void Attack()
    {
        if (enemy.state != EnemyState.Attacking)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            return;
        }        
        if (attackState == AttackState.None)
        {
            attackTimer = 0;
            attackState = AttackState.Hitting;
        }
        if (attackState == AttackState.Hitting)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
            if (attackTimer >= enemy.attackDelay / enemy.attackSpeed)
            {
                if (enemy.attackType == AttackType.Melee) HitPlayer();
                else LaunchBullet();
                attackState = AttackState.Cooldowning;
                return;
            }
        }
        if (attackState == AttackState.Cooldowning)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            if (attackTimer >= enemy.attackCooldown / enemy.attackSpeed)
            {
                attackState = AttackState.None;
                enemy.state = EnemyState.Chasing;
                return;
            }
        }
    }
    private void HitPlayer()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= enemy.attackRange + 1f)
        {
            player.GetComponent<Player>().Hit(25, enemy);
        }
    }
    private void LaunchBullet()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        GameObject bullet = Instantiate(Resources.Load<GameObject>("Projective"), transform.position, transform.rotation);
        bullet.AddComponent<Bullet>().Launch(direction, 15f, GetComponent<Enemy>());
    }
}
