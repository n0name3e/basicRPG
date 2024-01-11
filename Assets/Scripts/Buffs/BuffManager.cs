using UnityEngine;

public class BuffManager: MonoBehaviour
{
	public static BuffManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else 
		{
			Destroy(gameObject);
		}
	}
	public void AddBuff(Buff buff, IDamageable target)
	{
		target.buffs.Add(buff);
		buff.OnAddBuff?.Invoke(buff, target);
	}
}