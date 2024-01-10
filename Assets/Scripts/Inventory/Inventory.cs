using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private Player player;
    public List<Item> items { get; private set; } = new List<Item>();

    private void Awake()
    {
        player = GetComponent<Player>();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Update()
    {
        print(items.Count);
    }
    public void AddItem(Item item)
    {
        items.Add(item);
        ApplyItemStats(item);
        UI.Instance.UpdateItems();
    }
    public void RemoveItem(Item item)
    {
        items.Remove(item);
        RemoveItemStats(item);
        UI.Instance.UpdateItems();
    }
    public Item FindItem(string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Name == name) return items[i];
        }
        return null;
    }
    private void ApplyItemStats(Item item)
    {
        PlayerStats stats = player.PlayerStats;
        stats.physicDamage += item.gainedStats.physicDamage;
    }
    private void RemoveItemStats(Item item)
    {
        PlayerStats stats = player.PlayerStats;
        stats.physicDamage -= item.gainedStats.physicDamage;
    }
}
