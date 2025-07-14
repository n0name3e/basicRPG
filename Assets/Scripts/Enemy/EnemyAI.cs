using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    protected Enemy enemy;
    protected EnemyPatrolling patrolling;
    protected EnemyAttacking attacking;

    protected Transform player;

    private void OnEnable()
    {
        enemy = GetComponent<Enemy>();
        patrolling = GetComponent<EnemyPatrolling>();
        attacking = GetComponent<EnemyAttacking>();
        player = FindObjectOfType<Player>().transform;
    }
    protected virtual void Update()
    {
        if (enemy.State == EnemyState.Knockbacked || enemy.State == EnemyState.Stunned)
        {
            BreakAttack();
            return;
        }
        if (enemy.State == EnemyState.Patrolling)
            Patrol();
        if (enemy.State == EnemyState.Chasing)
            Chase();
        if (enemy.State == EnemyState.Attacking)
            Attack();
    }

    public virtual void Patrol()
    {
        patrolling.Patrol();
    }
    public virtual void Chase()
    {
        attacking.Chase();
    }

    public virtual void Attack()
    {
        attacking.Attack();
    }
    /// <summary>
    /// resets state to Idle and breaks the attack (used when enemy is stunned/knockbacked)
    /// </summary>
    public abstract void BreakAttack();
}
