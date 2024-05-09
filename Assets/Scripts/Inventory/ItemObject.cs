using UnityEngine;

public class ItemObject: MonoBehaviour
{
	public Item item;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<Player>() != null)
		{
			print("collide");
			Inventory.Instance.AddItem(item);
			Destroy(gameObject);
		}
	}
}