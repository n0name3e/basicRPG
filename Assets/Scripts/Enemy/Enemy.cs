using UnityEngine;
using System.Collections.Generic;

public enum EnemyState
{
    Stunned, // cannot do anything
    Knockbacked,
    Patrolling,
    Chasing,
    Attacking
}
public enum AttackType
{
    Ranged,
    Melee
}
public class Enemy : MonoBehaviour, IDamageable
{
    public float maxHealth { get; set; } = 100;
    public float Health { get; set; }
    public float speed { get; set; } = 2;

    private float stunDuration = 0f;
    public bool IsStunned { get { return stunDuration > 0f; } }


    public AttackType attackType = AttackType.Ranged;
    public float attackDelay = 0.5f;
    public float attackCooldown = 1f;
    public float attackSpeed = 1f; // multiplier to attack delay and cooldown
    public float attackRange = 4f; // distance at which enemy starts attacking
    public float attackDistance = 2.5f; // distance at which the attack would hit
    public float detectionRange = 6f;
    public float damage = 20f;
    public int experienceReward = 150;

    public AnimationCurve knockbackCurve;
    private Vector2 originalKnockbackPosition;
    private Vector2 knockbackDirection;
    private float knockbackDistance;
    private float knockbackDuration = 0f;
    private float knockbackTimer = 0f;

    public EnemyState State;
    private Player player;
    private Rigidbody2D rb;
    public Transform Transform { get; set; }


    //[SerializeField] public Dictionary<string, float> dropTable = new Dictionary<string, float>();
    //public SerializedDictionary dropTable { get; set; } = new SerializedDictionary(); //<string, float> dropTable;
    public SerializableDictionaryObject dropTable;
    public List<Buff> buffs { get; set; } = new List<Buff>();

    public void Hit(float damage, IDamageable sender)
    {
        Health -= damage;
        UI.Instance.UpdateEnemyHpBar(this);
        UI.Instance.CreateDamageText(transform.position, Mathf.CeilToInt(damage));
        if (Health <= 0) Die();
    }
    private void Die()
    {
        DropItems();
        Destroy(gameObject);
        player.AddXP(experienceReward);
    }
    public void Heal(float amount)
    {
        Health = Mathf.Min(maxHealth, Health + amount);
    }
    private void DropItems()
	{
        for (int i = 0; i < dropTable.keys.Count; i++)
        {
            if (Random.Range(0, 1) < dropTable.values[i])
            {
                GameObject box = Instantiate(Resources.Load<GameObject>("ItemBox"), transform.position, Quaternion.identity);
                string name = dropTable.keys[i];
                box.AddComponent<ItemObject>().item = ItemManager.Instance.FindItem(name);
            }
        }
	}
    private void OnEnable()
    {
        State = EnemyState.Patrolling;
        player = FindObjectOfType<Player>();
        Health = maxHealth;
        Transform = transform;
        rb = GetComponent<Rigidbody2D>();
        knockbackCurve = player.GetComponent<PlayerMovement>().knockbackCurve;
    }
    private void Update()
    {
        BuffManager.Instance.UpdateBuffs(this);
        if (State == EnemyState.Knockbacked)
        {
            return;
        }
        if (State == EnemyState.Stunned)
        {
            stunDuration -= Time.deltaTime;
            if (stunDuration <= 0f)
            {
                State = EnemyState.Patrolling;
            }
            return;
        }
        if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            State = EnemyState.Attacking;
            return;
        }
        if (State != EnemyState.Attacking && Vector2.Distance(transform.position, player.transform.position) <= detectionRange)
        {
            State = EnemyState.Chasing;
        }
        /*if (State == EnemyState.Attacking && Vector2.Distance(transform.position, player.transform.position) > attackRange + 1f)
        {
            State = EnemyState.Chasing;
        }*/
    }
    public void FixedUpdate()
    {
        if (State == EnemyState.Knockbacked)
        {
            float t = knockbackTimer / knockbackDuration;
            float eval = knockbackCurve.Evaluate(t);
            UI.Instance.debugText.text = eval.ToString();
            rb.MovePosition(Vector2.Lerp(originalKnockbackPosition, 
                originalKnockbackPosition + knockbackDirection, eval));
            //transform.position = originalKnockbackPosition +
            //    knockbackDirection * eval;

            knockbackTimer += Time.fixedDeltaTime;
            if (knockbackTimer >= knockbackDuration)
                State = EnemyState.Patrolling;
        }
    }
    public void Stun(float duration)
    {
        State = EnemyState.Stunned;
        stunDuration = Mathf.Max(stunDuration, duration);
        GetComponent<SpriteRenderer>().color = Color.grey;
    }

    public void Knockback(Vector2 direction, float distance, float duration = 0.5f)
    {
        print("knockbacking");
        //knockbackCurve = player.GetComponent<PlayerMovement>().knockbackCurve;
        knockbackTimer = 0f;
        knockbackDuration = duration;
        originalKnockbackPosition = transform.position;
        knockbackDirection = direction.normalized * distance;
        State = EnemyState.Knockbacked;
    }
}
