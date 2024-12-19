using UnityEngine;

public enum PlayerMovementState
{
    Moving,
    Knockbacked
}
public class PlayerMovement : MonoBehaviour
{
    private Player player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AnimationCurve knockbackCurve;
    public PlayerMovementState movementState;

    private Vector2 originalKnockbackPosition;
    private Vector2 knockbackDirection;
    private float knockbackTimer = 0f;
    
    private float attackTimer = 0f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        Move();
        attackTimer -= Time.deltaTime;
    }

    private void Move()
    {
        if (movementState == PlayerMovementState.Knockbacked)
        {
            transform.position = Vector3.Lerp(originalKnockbackPosition, knockbackDirection, knockbackCurve.Evaluate(knockbackTimer * 2.5f));
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer >= 1f/2.5f) movementState = PlayerMovementState.Moving;
            return;
        }
        float movementSpeed = player.PlayerStats.speed;
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, movementSpeed * Time.deltaTime, 0); // y++
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-movementSpeed * Time.deltaTime, 0, 0); // x--
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -movementSpeed * Time.deltaTime, 0); // y--
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(movementSpeed * Time.deltaTime, 0, 0); // x++
        }
        if(Input.GetMouseButtonDown(0))
        {
            //if (player.weapon.isMelee)

            Shoot();
        }
    }
    private void CheckMove()
    {

    }
    public void Knockback(Vector2 direction)
    {
        knockbackTimer = 0f;
        originalKnockbackPosition = transform.position;
        knockbackDirection = direction.normalized;
        knockbackDirection = new Vector2(transform.position.x, transform.position.y) + knockbackDirection;
        movementState = PlayerMovementState.Knockbacked;
    }
    private void Attack()
    {

    }
    private void Shoot()
    {
        if (attackTimer >= 0) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = (mousePosition - transform.position).normalized;

        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bullet = bulletObject.AddComponent<Bullet>();
        bullet.Launch(direction, 20f, GetComponent<IDamageable>(), player.PlayerStats.physicDamage);
        bullet.OnHit = player.weapon.OnHit;
        attackTimer = player.PlayerStats.attackCooldown / player.PlayerStats.attackSpeed;
        UI.Instance.UpdateAttackBar();
    }
}
