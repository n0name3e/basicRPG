using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartingInventory", menuName = "Inventory/Starting Inventory", order = 1)]
public class StartingInventory : ScriptableObject
{
    public List<Item> items = new List<Item>();
}
