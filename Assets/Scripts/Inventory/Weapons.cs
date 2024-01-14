using UnityEngine;

public class Weapons: MonoBehaviour
{
	public Weapon basicStaff;
	public Weapon fireStaff;
	private void Start()
	{
		
	}
	private void FireStaffHit(Enemy target)
	{
		Buff fire = new Buff("FireStaffFire", 2, target);
		Buff fireTimer = new Buff("FireStaffFireTimer", 0.5f, target, true);
		void FireStaffFireTimerRemove(Buff buff, IDamageable target)
		{
			
		}
		fireTimer.OnRemoveBuff = delegate {
			
		}
		fire.OnAddBuff = delegate
		{
			BuffManager.Instance.AddBuff(fireTimer, target);
		}
		BuffManager.Instance.AddBuff
	}
}