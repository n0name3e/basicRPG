using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    MaxHealth,
    MaxMana,
    MaxStamina,
    ManaRegen,
    StaminaRegen,
    Speed,
    Defense,
    AttackSpeed,
    AttackCooldown,
    PhysicalDamage,
    MagicalDamage,
    DashCooldown
}

public class PlayerStats: MonoBehaviour
{
    public Dictionary<StatType, float> Stats = new Dictionary<StatType, float>();
    public Dictionary<StatType, float> BonusStats = new Dictionary<StatType, float>(); // modifiers
    private Player player;
    
    public float MaxHealth
    {
        get { return GetStat(StatType.MaxHealth); }
        set { SetStat(StatType.MaxHealth, value); }
    }
    public float MaxMana
    {
        get { return GetStat(StatType.MaxMana); }
        set { SetStat(StatType.MaxMana, value); }
    }
    public float MaxStamina
    {
        get { return GetStat(StatType.MaxStamina); }
        set { SetStat(StatType.MaxStamina, value); }
    }
    public float ManaRegen { get { return GetStat(StatType.ManaRegen); } set { SetStat(StatType.ManaRegen, value); } }
    public float StaminaRegen { get { return GetStat(StatType.StaminaRegen); } set { SetStat(StatType.StaminaRegen, value); } }
    public float Speed { get { return GetStat(StatType.Speed); } set { Stats[StatType.Speed] = value; } }
    public float AttackSpeed { get { return GetStat(StatType.AttackSpeed); } set { Stats[StatType.AttackSpeed] = value; } }
    public float AttackCooldown { get { return GetStat(StatType.AttackCooldown); } set { Stats[StatType.AttackCooldown] = value; } }
    public float PhysicalDamage { get { return GetStat(StatType.PhysicalDamage); } set { Stats[StatType.PhysicalDamage] = value; } }
    public float MagicalDamage { get { return GetStat(StatType.MagicalDamage); } set { Stats[StatType.MagicalDamage] = value; } }
    public float Defense { get { return GetStat(StatType.Defense); } set { Stats[StatType.Defense] = value; } }
    public float DashCooldown { get { return GetStat(StatType.DashCooldown); } set { Stats[StatType.DashCooldown] = value; } }

    

    private void Awake()
    {
        foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
        {
            Stats[statType] = 0f;
            BonusStats[statType] = 0f;
        }
        player = GetComponent<Player>();
    }
    public float GetStat(StatType statType)
    {
        return Stats[statType] + BonusStats[statType];
    }
    public float GetBaseStat(StatType statType)
    {
        return Stats[statType];
    }
    public float GetBonusStat(StatType statType)
    {
        return BonusStats[statType];
    }
    public void SetStat(StatType statType, float value)
    {
        Stats[statType] = value;
        UpdateNecessaryStats();
    }
    /// <summary>
    /// Modifies bonus (not base) stat of the player
    /// </summary>
    public void ModifyStat(StatType statType, float value)
    {
        //Stats[statType] += value;
        BonusStats[statType] += value;
        UpdateNecessaryStats();
    }

    // i dont care about performance
    private void UpdateNecessaryStats()
    {
        player.Health = Mathf.Min(player.Health, GetStat(StatType.MaxHealth));
        player.Mana = Mathf.Min(player.Mana, GetStat(StatType.MaxMana));
        player.Stamina = Mathf.Min(player.Stamina, GetStat(StatType.MaxStamina));

        UI.Instance.StatBars.UpdateHpBar();
        UI.Instance.StatBars.UpdateManaBar();
        UI.Instance.StatBars.UpdateStaminaBar();
    }
}
