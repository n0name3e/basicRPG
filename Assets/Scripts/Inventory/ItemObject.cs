using UnityEngine;

public class ItemObject: MonoBehaviour
{
	public Item item;

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.transform.GetComponent<Player>() != null)
		{
            collision.transform.GetComponent<Player>().inventory.AddItem(item);
			Destroy(gameObject);
		}
	}
}