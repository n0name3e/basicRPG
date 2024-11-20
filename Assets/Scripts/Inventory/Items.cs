using System.Collections.Generic;
using UnityEngine;

public class Items: MonoBehaviour
{
    public static Items Instance { get; private set; }

    public Item stone;
    public Item healingPotion;
    public Equipment helm;

	private List<Item> items = new List<Item>();
	

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        healingPotion.OnUse.AddListener(UseHealingPotion);
		items.Add(stone);
		items.Add(healingPotion);
        items.Add(helm);
    }
    private void UseHealingPotion(GameObject player)
    {
        player.GetComponent<Player>().Heal(10);
    }
	public Item FindItem(string name)
	{
		for(int i = 0; i < items.Count; i++)
		{
			if (items[i].Name.ToLower() == name.ToLower()) return items[i];
		}
        return null;
	}
}
