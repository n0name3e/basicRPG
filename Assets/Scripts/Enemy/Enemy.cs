using UnityEngine;
using System.Collections.Generic;

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
    public float Health { get; set; }
    public float speed { get; set; } = 2;

    [field: SerializeField] public AttackType attackType = AttackType.Ranged;
    [field: SerializeField] public float attackDelay { get; set; } = 0.5f;
    [field: SerializeField] public float attackCooldown { get; set; } = 1f;
    [field: SerializeField] public float attackSpeed { get; set; } = 1f; // multiplier to attack delay and cooldown
    [field: SerializeField] public float attackRange { get; set; } = 2.5f;
    [field: SerializeField] public float damage { get; set; } = 20f;
    [field: SerializeField] public int experience = 150;

    [SerializeField] private float detectionRadius = 5f;

    public EnemyState state { get; set; }
    private Player player;
    public Transform Transform { get; set; }


    //[SerializeField] public Dictionary<string, float> dropTable = new Dictionary<string, float>();
    //public SerializedDictionary dropTable { get; set; } = new SerializedDictionary(); //<string, float> dropTable;
    public SerializableDictionaryObject dropTable;
    public List<Buff> buffs { get; set; } = new List<Buff>();

    public void Hit(float damage, IDamageable sender)
    {
        Health -= damage;
        UI.Instance.UpdateEnemyHpBar(this);
        CreateDamageText(Mathf.CeilToInt(damage));
        if (Health <= 0) Die();
    }
    private void CreateDamageText(int damage)
    {
        GameObject textObject = Instantiate(new GameObject("-" + damage), transform.position + new Vector3(0, 4), Quaternion.identity);
        UnityEngine.UI.Text text = textObject.AddComponent<UnityEngine.UI.Text>();
        text.text = damage.ToString();
    }
    private void Die()
    {
        DropItems();
        Destroy(gameObject);
        player.AddXP(experience);
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
		//for (int i = 0; i < dropTable.Count; i++)
        /*foreach (KeyValuePair<string, float> item in dropTable.dictionary)
		{
            print("dropping");
			if (Random.Range(0, 1) < dropTable.dictionary[item.Key])
			{
				GameObject box = Instantiate(Resources.Load<GameObject>("ItemBox"), transform.position, Quaternion.identity);
                string name = item.Key;
                print(box);
                box.AddComponent<ItemObject>().item = Items.Instance.FindItem(name);            
			}
		}*/
	}
    private void Start()
    {
        state = EnemyState.Patrolling;
        player = FindObjectOfType<Player>();
        Health = maxHealth;
        Transform = transform;
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
        BuffManager.Instance.UpdateBuffs(this);
    }
}
