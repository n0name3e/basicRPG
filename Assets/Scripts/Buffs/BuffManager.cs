using System.Collections.Generic;
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
	public void AddBuff(Buff buff, IDamageable target, bool triggerEvent = true)
	{
		//print($"{target.Transform.name} had their debuff { buff.name } Added! ");
		Buff existingBuff = FindBuff(buff.name, target);
		if (existingBuff != null)
		{
			existingBuff.TriggerUpdateEvent();
			existingBuff.time = buff.duration;
			return;
		}
		buff.time = buff.duration;
		target.buffs.Add(buff);
		if (triggerEvent)
			buff.TriggerAddEvent();
	}
	public void RemoveBuff(Buff buff, IDamageable target, bool triggerEvent = true)
	{
		target.buffs.Remove(FindBuff(buff.name, target));
		if (triggerEvent) 
			buff.TriggerRemoveEvent();
	}
	public Buff FindBuff(string name, IDamageable target)
	{
		List<Buff> buffs = target.buffs;
		for (int i = 0; i < buffs.Count; i++)
		{
			if (buffs[i].name == name) return buffs[i];
		}
		return null;
	}
	public void UpdateBuffs(IDamageable target)
	{
		List<Buff> buffs = target.buffs;
		for (int i = 0; i < buffs.Count; i++)
		{
			buffs[i].TriggerFrameUpdateEvent();
			if (buffs[i].unlimited) continue;
			buffs[i].time -= Time.deltaTime;
			if (buffs[i].time <= 0)
			{
				RemoveBuff(buffs[i], target);
			}
		}
	}
}