using UnityEngine;

public enum EnemyState
{
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
    public float health { get; set; }
    public float speed { get; set; } = 2;

    [field: SerializeField] public AttackType attackType = AttackType.Ranged;
    [field: SerializeField] public float attackDelay { get; set; } = 0.5f;
    [field: SerializeField] public float attackCooldown { get; set; } = 1f;
    [field: SerializeField] public float attackSpeed { get; set; } = 1f; // multiplier to attack delay and cooldown
    [field: SerializeField] public float attackRange { get; set; } = 2.5f;
    [field: SerializeField] public int experience = 150;

    [SerializeField] private float detectionRadius = 5f;

    public EnemyState state { get; set; }
    private Player player;
    public Transform Pos { get; set; }

	public Dictionary<string, int> dropTable = new Dictionary<string, int>();
	public List<Buff> buffs = new List<Buff>();
	

    public void Hit(float damage, IDamageable sender)
    {
        health -= damage;
        UI.Instance.UpdateEnemyHpBar(this);
        if (health <= 0) Die();
    }
    private void Die()
    {
        Destroy(gameObject);
        player.AddXP(experience);
    }
	private void DropItems()
	{
		for (int i = 0; i < dropTable.Count; i++)
		{
			float r = Random.Range(0, 1);
			if (r < dropTable[i].Value)
			{
				GameObject box = Instantiate(Resources.Load("ItemBox"), transform.position, Quaternion.identity);
			    box.AddComponent<ItemObject>().item = Items.FindItem(dropTable[i].Key);				
			}
		}
	}
    private void Start()
    {
        state = EnemyState.Patrolling;
        player = FindObjectOfType<Player>();
        health = maxHealth;
        Pos = transform;
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            state = EnemyState.Attacking;
            return;
        }
        if (state != EnemyState.Attacking && Vector2.Distance(transform.position, player.transform.position) <= detectionRadius)
        {
            state = EnemyState.Chasing;
        }
    }
}
