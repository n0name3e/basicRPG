using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IDamageable
{

    private int nextLevelExperience;
    private float health;
    public float Health { get => health; set => health = value; }
    public List<Ability> Abilities { get; set; } = new List<Ability>();
    public int Level { get; set; } = 1;
    public int Experience { get; private set; } = 0;
    public PlayerStats PlayerStats { get; private set; }
    private StartingPlayerStats startingPlayerStats;

    public List<Buff> buffs { get; set; } = new List<Buff>();

    private PlayerMovement _movement;

    public Transform Transform { get; set; }

    public Weapon weapon { get; private set; }



    private void Awake()
    {
        PlayerStats = GetComponent<PlayerStats>();
        _movement = GetComponent<PlayerMovement>();
        startingPlayerStats = GetComponent<StartingPlayerStats>();
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
        SetStartingStats();
        Health = PlayerStats.maxHealth;
    }
    private void Update()
    {
        BuffManager.Instance.UpdateBuffs(this);
    }

    public void Hit(float damage, IDamageable attacker)
    {
        float trueDamage = damage - PlayerStats.defense;
        trueDamage = (trueDamage < 1) ? 1 : trueDamage;
        Health -= trueDamage;
        UI.Instance.UpdateHealthBar();
        _movement.Knockback(transform.position - attacker.Transform.transform.position);
        if (Health <= 0) Destroy(gameObject);
    }
    public void Heal(float amount)
    {
        Health = Mathf.Min(PlayerStats.maxHealth, Health + amount);
        UI.Instance.UpdateHealthBar();
    }
    public void ChangeWeapon(Weapon weapon)
    {
        PlayerStats.attackCooldown = weapon.attackCooldown;
        PlayerStats.physicDamage = weapon.damage;
        print(weapon == null);
        this.weapon = weapon;
        this.weapon.OnHit = weapon.OnHit;
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
    public void SetStartingStats()
    {
        PlayerStats.maxHealth = startingPlayerStats.maxHealth;
        PlayerStats.speed = startingPlayerStats.speed;
        PlayerStats.attackSpeed = startingPlayerStats.attackSpeed;
        PlayerStats.attackCooldown = startingPlayerStats.attackCooldown;
        PlayerStats.physicDamage = startingPlayerStats.physicDamage;
        PlayerStats.magicDamage = startingPlayerStats.magicDamage;
        PlayerStats.defense = startingPlayerStats.defense;
    }
}
