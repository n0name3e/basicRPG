using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Items Items; // Items

    public static ItemManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public Item FindItem(string name)
    {
        for (int i = 0; i < Items.items.Count; i++)
        {
            if (Items.items[i].Name.ToLower() == name.ToLower()) return Items.items[i];
        }
        return null;
        //return items.items.Find(item => item.name == name);
    }
}
