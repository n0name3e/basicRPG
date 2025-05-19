using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private int nextLevelExperience;
    public float Health { get; set; }
    public float Mana { get; set; }
    public float Stamina { get; set; }
    public int Level { get; set; } = 1;
    public int Experience { get; private set; } = 0;
    public List<Ability> Abilities { get; set; } = new List<Ability>();

    private float manaRegenTimer = 0f;
    private float staminaRegenTimer = 0f;

    public bool meleeEquipped = true;
    public PlayerStats PlayerStats { get; private set; }
    private StartingPlayerStats startingPlayerStats;

    [SerializeField] private GameObject swordObject;

    public List<Buff> buffs { get; set; } = new List<Buff>();

    public Inventory inventory;
    public PlayerMovement movement;

    public Transform Transform { get; set; } // used to get gameobject of IDamageable

    //public Weapon weapon { get; private set; }



    private void Awake()
    {
        PlayerStats = GetComponent<PlayerStats>();
        movement = GetComponent<PlayerMovement>();
        inventory = GetComponent<Inventory>();
        startingPlayerStats = GetComponent<StartingPlayerStats>();
    }
    private void Start()
    {
        SetStartingStats();
        Health = PlayerStats.MaxHealth;
        Mana = PlayerStats.MaxMana;
        Stamina = PlayerStats.MaxStamina;
        UI.Instance.StatBars.UpdateHpBar();
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
    }
    private void Update()
    {
        BuffManager.Instance.UpdateBuffs(this);
        manaRegenTimer -= Time.deltaTime;
        if (movement.movementState == PlayerMovementState.Moving)
        {
            staminaRegenTimer -= Time.deltaTime;
        }
        if (staminaRegenTimer <= 0)
        {
            RegenerateStamina(1);
            staminaRegenTimer += 1 / PlayerStats.GetStat(StatType.StaminaRegen);
        }
        if (manaRegenTimer <= 0)
        {
            ReplenishMana(1);
            manaRegenTimer += 1 / PlayerStats.GetStat(StatType.ManaRegen);
        }
    }

    public void Hit(float damage, IDamageable attacker)
    {
        float trueDamage = damage - PlayerStats.GetStat(StatType.Defense);
        trueDamage = (trueDamage < 1) ? 1 : trueDamage;
        Health -= trueDamage;
        UI.Instance.StatBars.UpdateHpBar();

        movement.Knockback(transform.position - attacker.Transform.transform.position);
        if (Health <= 0) Destroy(gameObject);
    }
    public void HitWithSword(Enemy enemy)
    {
        inventory.equippedSword.HitTarget(PlayerStats.GetStat(StatType.PhysicalDamage), enemy, this);
    }
    public void Heal(float amount)
    {
        Health = Mathf.Min(PlayerStats.GetStat(StatType.MaxHealth), Health + amount);
        UI.Instance.StatBars.UpdateHpBar();
    }
    public void SpendMana(float amount)
    {
        Mana = Mathf.Min(PlayerStats.GetStat(StatType.MaxMana), Mana - amount);
        manaRegenTimer = 4f;
        UI.Instance.StatBars.UpdateManaBar();
    }
    public void ReplenishMana(float amount)
    {
        Mana = Mathf.Min(PlayerStats.GetStat(StatType.MaxMana), Mana + amount);
        UI.Instance.StatBars.UpdateManaBar();
    }
    public void ExpendStamina(float amount)
    {
        Stamina = Mathf.Min(PlayerStats.GetStat(StatType.MaxStamina), Stamina - amount);
        staminaRegenTimer = 0.5f;
        UI.Instance.StatBars.UpdateStaminaBar();
    }
    public void RegenerateStamina(float amount)
    {
        Stamina = Mathf.Min(PlayerStats.GetStat(StatType.MaxStamina), Stamina + amount);
        UI.Instance.StatBars.UpdateStaminaBar();
    }
    public void ChangeMeleeWeapon(Sword weapon)
    {       
        PlayerStats.SetStat(StatType.PhysicalDamage, weapon.physicalDamage);
        inventory.equippedSword = weapon;
        UI.Instance.ChangeWeapon(inventory.equippedSword);
    }
    public void ChangeMagicWeapon(Staff weapon)
    {
        PlayerStats.SetStat(StatType.MagicalDamage, weapon.magicDamage);
        PlayerStats.SetStat(StatType.AttackCooldown, weapon.attackCooldown);
        inventory.equippedStaff = weapon;
        UI.Instance.ChangeWeapon(inventory.equippedStaff);
    }
    public void EquipWeapon(bool isSword)
    {
        if (movement.movementState == PlayerMovementState.Attacking)
        {
            movement.movementState = PlayerMovementState.Moving;
        }
        //swordObject.GetComponent<SwordAnimatorHandler>().GetComponent<Animator>().Play("Idle");
        swordObject.SetActive(isSword);
        swordObject.transform.localRotation = Quaternion.identity;
        meleeEquipped = isSword;
    }
    public void AddAbility(Ability ability)
    {
        Abilities.Add(ability);
        AbilityManager.Instance.CreateAbilityButtons();
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
        PlayerStats.MaxHealth = startingPlayerStats.maxHealth;
        PlayerStats.MaxMana = startingPlayerStats.maxMana;
        PlayerStats.MaxStamina = startingPlayerStats.maxStamina;
        PlayerStats.ManaRegen = startingPlayerStats.manaRegen;
        PlayerStats.StaminaRegen = startingPlayerStats.staminaRegen;
        PlayerStats.Speed = startingPlayerStats.speed;
        PlayerStats.AttackSpeed = startingPlayerStats.attackSpeed;
        PlayerStats.AttackCooldown = startingPlayerStats.attackCooldown;
        PlayerStats.PhysicalDamage = startingPlayerStats.physicDamage;
        PlayerStats.MagicalDamage = startingPlayerStats.magicDamage;
        PlayerStats.Defense = startingPlayerStats.defense;
    }
}
