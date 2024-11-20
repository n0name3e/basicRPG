using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    MaxHealth,
    Speed,
    AttackSpeed,
    AttackCooldown,
    PhysicDamage,
    MagicDamage,
    Defense
}

public class PlayerStats: MonoBehaviour
{
    public Dictionary<StatType, float> Stats = new Dictionary<StatType, float>()
    {
        { StatType.MaxHealth, 0 },
        { StatType.Speed, 0 },
        { StatType.AttackSpeed, 0 },
        { StatType.AttackCooldown, 0 },
        { StatType.PhysicDamage, 0 },
        { StatType.MagicDamage, 0 },
        { StatType.Defense, 0 },
    };
    public float maxHealth { get { return Stats[StatType.MaxHealth]; } set 
        { 
            Stats[StatType.MaxHealth] = value;
            UI.Instance.UpdateHealthBar();
        } }
    public float speed { get { return Stats[StatType.Speed]; } set { Stats[StatType.Speed] = value; } }
    public float attackSpeed { get { return Stats[StatType.AttackSpeed]; } set { Stats[StatType.AttackSpeed] = value; } }
    public float attackCooldown { get { return Stats[StatType.AttackCooldown]; } set { Stats[StatType.AttackCooldown] = value; } }
    public float physicDamage { get { return Stats[StatType.PhysicDamage]; } set { Stats[StatType.PhysicDamage] = value; } }
    public float magicDamage { get { return Stats[StatType.MagicDamage]; } set { Stats[StatType.MagicDamage] = value; } }
    public float defense { get { return Stats[StatType.Defense]; } set { Stats[StatType.Defense] = value; } }
}
