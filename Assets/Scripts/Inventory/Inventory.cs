using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //public static Inventory Instance { get; private set; }
    private Player player;
    public List<Item> items { get; private set; } = new List<Item>();
	public List<Weapon> weapons { get; private set; } = new List<Weapon>();
    public Sword equippedSword;
    public Staff equippedStaff;
    public StartingInventory StartingInventory;
    public Dictionary<EquipmentSlot, Equipment> equipment = new Dictionary<EquipmentSlot, Equipment>()
    {
        { EquipmentSlot.Head, null },
        { EquipmentSlot.Body, null },
        { EquipmentSlot.Legs, null }
    };
    

    private void Awake()
    {
        player = GetComponent<Player>();
        //if (Instance == null) Instance = this;
        //else Destroy(gameObject);
    }
    private IEnumerator Start()
    {
        yield return null;
        for (int i = 0; i < StartingInventory.items.Count; i++)
        {
            AddItem(StartingInventory.items[i]);
        }
    }
    public void AddItem(Item item)
    {
        items.Add(item);
        UI.Instance.UpdateItems();
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        UI.Instance.UpdateItems();
    }
    public void ChangeMeleeWeapon(Sword weapon)
    {
        AddItem(equippedSword);
        equippedSword = weapon;
        player.ChangeMeleeWeapon(weapon);
        UI.Instance.ChangeWeapon(weapon);
    }
    public void ChangeMagicWeapon(Staff weapon)
    {
        AddItem(equippedStaff);
        equippedStaff = weapon;
        player.ChangeMagicWeapon(weapon);
        UI.Instance.ChangeWeapon(weapon);
    }
	/*public void ChangeWeapon(Weapon weapon)
	{
        AddItem(equippedWeapon);
        equippedWeapon = weapon;
		player.ChangeWeapon(weapon);
        UI.Instance.ChangeWeapon();
    }*/
    public Item FindItem(string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Name == name) return items[i];
        }
        return null;
    }
    public void EquipArmor(Equipment armor)
    {
        if (equipment[armor.equipmentSlot] != null)
        {
            UnEquipArmor(armor.equipmentSlot);
        }
        equipment[armor.equipmentSlot] = armor;
        player.PlayerStats.ModifyStat(StatType.Defense, armor.Defense);
        ApplyArmorStats(armor);
        UI.Instance.UpdateArmor(armor.equipmentSlot);
    }
    public void UnEquipArmor(EquipmentSlot slot)
    {
        Equipment armor = equipment[slot];
        player.PlayerStats.ModifyStat(StatType.Defense, -armor.Defense);
        RemoveArmorStats(armor);
        AddItem(armor);
        equipment[slot] = null;
    }
    private void ApplyArmorStats(Equipment armor)
    {
        Dictionary<StatType, float> stats = player.PlayerStats.Stats;
        Dictionary<StatType, float> armorStats = armor.gainedStats;
        for (int i = 0; i < armorStats.Count; i++)
        {
            StatType type = armorStats.ElementAt(i).Key;
            player.PlayerStats.ModifyStat(type, armorStats[type]);
            //stats[type] += armorStats[type];
        }
        UI.Instance.UpdatePlayerStats();
    }
    private void RemoveArmorStats(Equipment armor)
    {
        Dictionary<StatType, float> stats = player.PlayerStats.Stats;
        Dictionary<StatType, float> armorStats = armor.gainedStats;
        for (int i = 0; i < armorStats.Count; i++)
        {
            StatType type = armorStats.ElementAt(i).Key;
            player.PlayerStats.ModifyStat(type, -armorStats[type]);
            //stats[type] -= armorStats[type];
        }
        UI.Instance.UpdatePlayerStats();
        UI.Instance.StatBars.UpdateHpBar();
    }
}
