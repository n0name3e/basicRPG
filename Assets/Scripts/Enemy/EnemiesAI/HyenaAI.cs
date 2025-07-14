using UnityEngine;

public class HyenaAI : EnemyAI
{
    private SpriteRenderer spriteRenderer;

    private enum State
    {
        Idle, // patrolling or chasing
        Charging,
        Dashing,
        Attacking,
        Recovering
    }
    private State currentState = State.Idle;
    private Vector2 dashDirection;

    [SerializeField] private float chargeTime = 0.25f; // time before dash starts
    [SerializeField] private float dashTime = 0.5f; // time of dash
    [SerializeField] private float attackTime = 0.25f; // time before attack after dash
    [SerializeField] private float recoveryTime = 1.25f; // time of recovery after attack
    [SerializeField] private float dashSpeedMultiplier = 3.2f; // speed multiplier during dash

    private float chargeTimer; // timer of idle before dash
    private float dashTimer; // timer of dashing after charge and before attack
    private float attackTimer; // timer after which attack is made
    private float recoveryTimer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Attack() // called each frame
    {
        if (currentState == State.Idle)
        {
            dashDirection = (player.position - transform.position).normalized;
            spriteRenderer.color = Color.white;
            chargeTimer = chargeTime;
            currentState = State.Charging;
            return;
        }
        if (currentState == State.Charging)
        {
            spriteRenderer.color = Color.yellow;
            chargeTimer -= Time.deltaTime;
            if (chargeTimer <= 0)
            {
                currentState = State.Dashing;
                dashTimer = dashTime; // duration of dash
            }
        }
        if (currentState == State.Dashing)
        {
            spriteRenderer.color = Color.cyan;
            transform.position += (Vector3)dashDirection * enemy.speed * dashSpeedMultiplier * Time.deltaTime;
            dashTimer -= Time.deltaTime;
            if (Vector2.Distance(transform.position, player.position) <= enemy.attackDistance / 1.7f)
            {
                dashTimer = 0; // stop dashing if close enough
            }
            if (dashTimer <= 0)
            {
                if (Vector2.Distance(transform.position, player.transform.position) >= enemy.attackDistance * 2)
                {
                    currentState = State.Recovering;
                    recoveryTimer = recoveryTime / 2f;
                    return;
                }
                currentState = State.Attacking;
                attackTimer = attackTime; // time before attack
            }
        }
        if (currentState == State.Attacking)
        {
            spriteRenderer.color = Color.red;
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                if (Vector2.Distance(transform.position, player.transform.position) <= enemy.attackDistance)
                {
                    player.GetComponent<IDamageable>().Hit(enemy.damage, enemy);
                }
                recoveryTimer = recoveryTime;
                currentState = State.Recovering;
            }
        }
        if (currentState == State.Recovering)
        {
            spriteRenderer.color = Color.green;
            recoveryTimer -= Time.deltaTime;
            if (recoveryTimer <= 0)
            {
                currentState = State.Idle;
                enemy.State = EnemyState.Chasing;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (enemy.State != EnemyState.Attacking)
        {
            spriteRenderer.color = Color.white;
        }
    }
    public override void BreakAttack()
    {
        currentState = State.Idle;
    }
}
