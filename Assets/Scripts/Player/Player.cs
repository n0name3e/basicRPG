using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IDamageable
{

    private int nextLevelExperience;
    public float health { get; set; }
    public List<Ability> Abilities { get; set; } = new List<Ability>();
    public int Level { get; set; } = 1;
    public int Experience { get; private set; } = 0;
    public PlayerStats PlayerStats { get; private set; }

	public List<Buff> buffs { get; set; } = new List<Buff>();

    private PlayerMovement _movement;
    public Transform Transform { get; set; }

    public Weapon weapon { get; private set; }



    private void Awake()
    {
        PlayerStats = GetComponent<PlayerStats>();
        _movement = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        Transform = transform;
        /*Buff healthRegenTimer = new Buff("timer", 1f, this);
        healthRegenTimer.OnRemoveBuff = delegate
        {
            Heal(1);
            BuffManager.Instance.AddBuff(healthRegenTimer, this);
        };
        Buff healthRegen = new Buff("hpRegen", 1f, this)
        {
            unlimited = true,
            OnAddBuff = delegate 
            {
                BuffManager.Instance.AddBuff(healthRegenTimer, this);
            }
        };
        BuffManager.Instance.AddBuff(healthRegen, this);
        */
        health = PlayerStats.maxHealth;
    }
	private void Update()
	{
		BuffManager.Instance.UpdateBuffs(this);
	}

    public void Hit(float damage, IDamageable attacker)
    {
        health -= damage;
        UI.Instance.UpdateHealthBar();
        if (health <= 0) Destroy(gameObject);
        _movement.Knockback(transform.position - attacker.Transform.transform.position);
    }
    public void Heal(float amount)
    {
        health = Mathf.Min(PlayerStats.maxHealth, health + amount);
        UI.Instance.UpdateHealthBar();
    }
	public void ChangeWeapon(Weapon weapon)
	{
		PlayerStats.attackCooldown = weapon.attackCooldown;
        PlayerStats.physicDamage = weapon.damage;
        this.weapon = weapon;
	}
    public void AddXP(int xp)
    {
        Experience += xp;
        CheckLevel();     
    }
    private void CheckLevel()
    {
        if (Experience >= nextLevelExperience)
        {
            UpLevel();
        }
    }
    private void UpLevel()
    {
        Level++;
        Experience -= nextLevelExperience;
        nextLevelExperience = GameManager.Instance.GetNextLevelExperience(Level);
    }
}
