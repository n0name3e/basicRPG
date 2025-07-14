using UnityEngine;
using UnityEngine.EventSystems;

public enum PlayerMovementState
{
    Moving,
    Attacking,
    Knockbacked,
    Dashing
}
public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    public SwordAnimatorHandler swordAnimatorHandler;
    [SerializeField] private GameObject bulletPrefab;
    [field: SerializeField] public AnimationCurve knockbackCurve { get; private set; }

    public PlayerMovementState movementState;
    public bool isAimingAbility;

    private Vector2 originalKnockbackPosition;
    private Vector2 knockbackDirection;
    private float knockbackTimer = 0f;

    private Vector3 originalDashPosition;
    private Vector3 dashDirection;
    private float dashTimer = 0f; // used for dash duration and cooldown
    
    private float attackTimer = 0f;
    private float playerAttackRotation = 0f;


    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        swordAnimatorHandler = GetComponentInChildren<SwordAnimatorHandler>();
    }
    private void Update()
    {
        //CheckMovementState();
        attackTimer -= Time.deltaTime;
        dashTimer += Time.deltaTime;
        Move();
        if (movementState != PlayerMovementState.Attacking)
        {
            RotateTowardsMouse();
        }
        else // to fix the stupid bug where the player rotates while attacking (it could actually happened because of z-axis on rb wasn't frozen)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, playerAttackRotation));
        }
        if (isAimingAbility && movementState != PlayerMovementState.Moving)
        {
            isAimingAbility = false;
            AbilityManager.Instance.CancelTargeting();
        }
        if (Input.GetMouseButtonDown(0) && !isAimingAbility && movementState == PlayerMovementState.Moving
            && !EventSystem.current.IsPointerOverGameObject())
        {
            if (player.meleeEquipped)
                Attack();
            else 
                Shoot();
        }
    }
    private void FixedUpdate()
    {
        CheckMovementState();
    }
    private void CheckMovementState()
    {
        if (movementState == PlayerMovementState.Knockbacked)
        {
            rb.MovePosition(Vector2.Lerp(originalKnockbackPosition, 
                (originalKnockbackPosition + knockbackDirection * (player.PlayerStats.Speed / 2f)), knockbackCurve.Evaluate(knockbackTimer * 2.5f)));

            knockbackTimer += Time.fixedDeltaTime;
            if (knockbackTimer >= 1f / 2.5f) 
                movementState = PlayerMovementState.Moving;
            return;
        }
        if (movementState == PlayerMovementState.Dashing)
        {
            rb.MovePosition(Vector2.Lerp(originalDashPosition,
               (originalDashPosition + dashDirection * (player.PlayerStats.Speed / 2f)), knockbackCurve.Evaluate(dashTimer * 4f)));
            if (dashTimer >= 1f / 4f)
                movementState = PlayerMovementState.Moving;
            return;
        }

        // otherwise just move
        rb.MovePosition(rb.position + (GetMovementDirection() * player.PlayerStats.Speed * Time.fixedDeltaTime));
    }
    private Vector2 GetMovementDirection()
    {
        float moveX = 0;
        float moveY = 0;
        if (Input.GetKey(KeyCode.W)) moveY += 1f;
        if (Input.GetKey(KeyCode.S)) moveY -= 1f;
        if (Input.GetKey(KeyCode.A)) moveX -= 1f;
        if (Input.GetKey(KeyCode.D)) moveX += 1f;

        return new Vector2(moveX, moveY).normalized;
    }
    public bool AimAbility() // true if can aim ability
    {   
        if (movementState != PlayerMovementState.Moving)
        {
            return false;
        }
        isAimingAbility = true;
        return true;
    }
    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.Space) && movementState == PlayerMovementState.Moving && dashTimer >= player.PlayerStats.DashCooldown)
        {
            dashTimer = 0f;
            originalDashPosition = transform.position;
            
            dashDirection = GetMovementDirection();
            movementState = PlayerMovementState.Dashing;
            return;
        }

        //transform.Translate(new Vector2(moveX, moveY).normalized * movementSpeed * Time.deltaTime);
    }

    public void Knockback(Vector2 direction)
    {
        knockbackTimer = 0f;
        originalKnockbackPosition = transform.position;
        knockbackDirection = direction.normalized;
        movementState = PlayerMovementState.Knockbacked;
    }
    private void RotateTowardsMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
        mousePos.x -= playerPos.x;
        mousePos.y -= playerPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    private void Attack() // for melee attack 
    {
        if (player.Stamina < player.inventory.equippedSword.staminaCost)
            return;

        Sword sword = player.inventory.equippedSword;
        float attackLength = swordAnimatorHandler.GetAnimationLength(sword.swingAnimationName);
        float targetAttackDuration = sword.attackDuration;
        float speedMultiplier = attackLength / targetAttackDuration * player.PlayerStats.AttackSpeed;

        player.ExpendStamina(player.inventory.equippedSword.staminaCost);

        playerAttackRotation = transform.rotation.eulerAngles.z;
        movementState = PlayerMovementState.Attacking;
        swordAnimatorHandler.PlayAnimation(player.inventory.equippedSword.swingAnimationName, speedMultiplier);     
    }
    
    private void Shoot()
    {
        if (attackTimer > 0 || player.Mana < player.inventory.equippedStaff.manaCost) 
            return;

        player.SpendMana(player.inventory.equippedStaff.manaCost);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = (mousePosition - transform.position).normalized;

        GameObject bulletObject = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Launch(direction, 20f, GetComponent<IDamageable>(), player.PlayerStats.MagicalDamage, player.inventory.equippedStaff);
        attackTimer = player.PlayerStats.AttackCooldown / player.PlayerStats.AttackSpeed;
        UI.Instance.UpdateAttackBar();
    }
}
