using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsDatabase", menuName = "Items/ItemsDB")]
public class Items: ScriptableObject
{
	public List<Item> items = new List<Item>();
}
