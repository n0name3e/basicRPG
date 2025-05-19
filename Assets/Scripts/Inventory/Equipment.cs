using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot
{
    Head,
    Body,
    Legs
}
[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Armor", order = 2)]
public class Equipment : Item
{
    public EquipmentSlot equipmentSlot;
    public float Defense;
    //public ItemStats gainedStats; //{ get; private set; }
    //public Dictionary<StatType, float> statsDictionary = new Dictionary<StatType, float>();
    public Dictionary<StatType, float> gainedStats = new Dictionary<StatType, float>();

    [SerializeField] private List<StatModifier> statModifiers;

    private void OnValidate()
    {
        gainedStats.Clear();
        for (int i = 0; i < statModifiers.Count; i++)
        {
            gainedStats.Add(statModifiers[i].type, statModifiers[i].value);
        }
    }

    /*private void OnEnable()
    {
        statsDictionary[StatType.Defense] = defense;
        statsDictionary[StatType.MaxHealth] = gainedStats.maxHealth;
        statsDictionary[StatType.Speed] = gainedStats.speed;
        statsDictionary[StatType.AttackSpeed] = gainedStats.attackSpeed;
        statsDictionary[StatType.AttackCooldown] = gainedStats.attackCooldown;
        statsDictionary[StatType.PhysicDamage] = gainedStats.physicDamage;
        statsDictionary[StatType.MagicDamage] = gainedStats.magicDamage;
    }*/
}
