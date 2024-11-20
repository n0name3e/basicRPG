using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot
{
    Head,
    Chest,
    Legs
}
[CreateAssetMenu(fileName = "New Armor", menuName = "Items/Armor", order = 2)]
public class Equipment : Item
{
    public EquipmentSlot equipmentSlot;
    [SerializeField] private float defense;
    public ItemStats gainedStats; //{ get; private set; }
    public Dictionary<StatType, float> statsDictionary = new Dictionary<StatType, float>();
    private void OnEnable()
    {
        statsDictionary[StatType.Defense] = defense;
        statsDictionary[StatType.MaxHealth] = gainedStats.maxHealth;
        statsDictionary[StatType.Speed] = gainedStats.speed;
        statsDictionary[StatType.AttackSpeed] = gainedStats.attackSpeed;
        statsDictionary[StatType.AttackCooldown] = gainedStats.attackCooldown;
        statsDictionary[StatType.PhysicDamage] = gainedStats.physicDamage;
        statsDictionary[StatType.MagicDamage] = gainedStats.magicDamage;
        MonoBehaviour.print(statsDictionary[StatType.MaxHealth]);
    }
}
