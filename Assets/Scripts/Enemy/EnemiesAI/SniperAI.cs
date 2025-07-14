using UnityEngine;

public class SniperAI : EnemyAI
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private enum State
    {
        Idle, // patrolling or chasing
        Dashing,
        Aiming,
        Recovering
    }
    private State currentState = State.Idle;
    private Vector2 shootDirection;
    private Vector2 lastPlayerPos;
    private bool isAiming = false;
    private bool hasAimed = false;

    [SerializeField] private float chargeTime = 1f; // time before attack is made
    [SerializeField] private float recoveryTime = 1.25f; // time of recovery after attack

    private float chargeTimer; // timer of idle before dash
    private float recoveryTimer;

    private Vector2 retreatDirection;
    private Vector2 startingRetreatingPosition;
    private AnimationCurve dashCurve;
    private float dashTimer = 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        dashCurve = GameManager.Instance.knockbackCurve;
    }
    private void FixedUpdate()
    {
        if (isAiming)
            lastPlayerPos = player.transform.position;

        if (currentState == State.Dashing)
        {
            rb.MovePosition(Vector2.Lerp(startingRetreatingPosition, 
                startingRetreatingPosition + retreatDirection * 3f, dashCurve.Evaluate(dashTimer * 2f)));
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= 0.5f)
                currentState = State.Aiming;
        }
    }

    public override void Attack() // called each frame
    {
        if (currentState == State.Idle)
        {
            spriteRenderer.color = Color.red;
            chargeTimer = chargeTime;
            if (Vector2.Distance(transform.position, player.position) < 2.5f)
            {
                currentState = State.Dashing;
                retreatDirection = CalculateRetreatDirection();
                startingRetreatingPosition = transform.position;
                dashTimer = 0f;
            }
            else
                currentState = State.Aiming;
            return;
        }
        if (currentState == State.Aiming)
        {
            chargeTimer -= Time.deltaTime;
            isAiming = true;
            if (!hasAimed && chargeTimer <= chargeTime / 2f)
            {
                shootDirection = CalculateLeadDirection();
                hasAimed = true;
                isAiming = false;
            }
            if (chargeTimer <= 0)
            {
                GameObject bullet = Instantiate(Resources.Load<GameObject>("Projectile"), transform.position, Quaternion.identity);

                bullet.GetComponent<Bullet>().Launch(shootDirection, 45f, GetComponent<Enemy>(), GetComponent<Enemy>().damage);

                hasAimed = false;
                currentState = State.Recovering;
                recoveryTimer = recoveryTime;
                spriteRenderer.color = Color.green;
            }
        }

        if (currentState == State.Recovering)
        {
            recoveryTimer -= Time.deltaTime;
            if (recoveryTimer <= 0)
            {
                spriteRenderer.color = Color.white;
                currentState = State.Idle;
                enemy.State = EnemyState.Chasing;
            }
        }
    }

    private Vector2 CalculateLeadDirection()
    {
        Vector2 playerVelocity = (player.transform.position - (Vector3)lastPlayerPos) / Time.deltaTime;
        Vector2 targetPosition = player.position + (Vector3)playerVelocity * Random.Range(0.1f, 0.3f);
        UI.Instance.debugText.text = (targetPosition - (Vector2)transform.position).normalized.x.ToString() + "; " +
            (targetPosition - (Vector2)transform.position).normalized.y.ToString();

        Debug.DrawLine(player.transform.position, player.position + (Vector3) playerVelocity, Color.red, 1f);
        lastPlayerPos = Vector2.zero;
        return (targetPosition - (Vector2)transform.position).normalized;
    }
    private Vector2 CalculateRetreatDirection()
    {
        Vector2 baseBehindDirection = (transform.position - player.position).normalized;
        float angle = Random.Range(-60, 60);

        return Quaternion.Euler(0, 0, angle) * baseBehindDirection;
    }

    protected override void Update()
    {
        base.Update();
        if (enemy.State != EnemyState.Attacking && enemy.State != EnemyState.Stunned)
        {
            spriteRenderer.color = Color.white;
        }
    }
    
    public override void BreakAttack()
    {
        currentState = State.Idle;
        isAiming = false;
    }
}
