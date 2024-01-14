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
		Buff existingBuff = FindBuff(buff.name);
		if (existingBuff != null)
		{
			existingBuff.Update();
			existingBuff.time = buff.duration;
			return;
		}
		target.buffs.Add(buff);
		buff.Add();
	}
	public void RemoveBuff(Buff buff, IDamageable target)
	{
		buff.Remove();
		target.buffs.Remove(FindBuff(buff.Name, target));
	}
	public Buff FindBuff(string name, IDamageable target)
	{
		List<Buff> buffs = target.buffs;
		for (int i = 0; i < buffs.Count; i++)
		{
			if buffs[i].Name == name) return buffs[i];
		}
		return null;
	}
	public void UpdateBuffs(IDamageable target)
	{
		List<Buff> buffs = target.buffs;
		for (int i = 0; i < buffs.Count; i++)
		{
			buffs[i].FrameUpdate();
			buffs[i].time -= Time.deltaTime;
			if (buffs[i].time <= 0)
			{
				RemoveBuff(buffs[i], target);
			}
		}
	}
}