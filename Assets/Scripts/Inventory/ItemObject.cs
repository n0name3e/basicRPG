using UnityEngine;

public class ItemObject: MonoBehaviour
{
	public Item item;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Player>() != null)
		{
			Inventory.Instance.AddItem(item);
		}
	}
}