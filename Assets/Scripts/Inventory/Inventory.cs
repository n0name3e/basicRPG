using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private Player player;
    public List<Item> items { get; private set; } = new List<Item>();
	public List<Weapon> weapons { get; private set; } = new List<Weapon>();
    public Dictionary<EquipmentSlot, Equipment> equipment = new Dictionary<EquipmentSlot, Equipment>()
    {
        { EquipmentSlot.Head, null },
        { EquipmentSlot.Chest, null },
        { EquipmentSlot.Legs, null }
    };
    

    private void Awake()
    {
        player = GetComponent<Player>();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void AddItem(Item item)
    {
        items.Add(item);
        //ApplyArmorStats(item);
        UI.Instance.UpdateItems();
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        //RemoveArmorStats(item);
        UI.Instance.UpdateItems();
    }
	public void ChangeWeapon(Weapon weapon)
	{
		player.ChangeWeapon(weapon);
	}
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
            AddItem(equipment[armor.equipmentSlot]);
        }
        equipment[armor.equipmentSlot] = armor;
        ApplyArmorStats(armor);
        UI.Instance.UpdateArmor(armor.equipmentSlot);
    }
    public void UnEquipArmor(EquipmentSlot slot)
    {
        Equipment armor = equipment[slot];
        RemoveArmorStats(armor);
        AddItem(armor);
        equipment[slot] = null;
    }
    private void ApplyArmorStats(Equipment armor)
    {
        Dictionary<StatType, float> stats = player.PlayerStats.Stats;
        Dictionary<StatType, float> armorStats = armor.statsDictionary;
        for (int i = 0; i < armor.statsDictionary.Count; i++)
        {
            StatType type = (StatType)i;
            stats[type] += armorStats[type];
        }
        UI.Instance.ShowPlayerStats();
        UI.Instance.UpdateHealthBar();
    }
    private void RemoveArmorStats(Equipment armor)
    {
        Dictionary<StatType, float> stats = player.PlayerStats.Stats;
        Dictionary<StatType, float> armorStats = armor.statsDictionary;
        for (int i = 0; i < armor.statsDictionary.Count; i++)
        {
            StatType type = (StatType)i;
            stats[type] -= armorStats[type];
        }
        UI.Instance.ShowPlayerStats();
        UI.Instance.UpdateHealthBar();
    }
}
