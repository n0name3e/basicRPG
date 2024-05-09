using UnityEngine;

public class Weapons: MonoBehaviour
{
	public Weapon basicStaff;
	public Weapon fireStaff;
	private System.Collections.IEnumerator Start()
	{
		/*fireStaff = new Weapon()
		{
			damage = 40,
			OnHit = FireStaffHit
		};*/
		yield return null;
		fireStaff.OnHit = FireStaffHit;
		GameManager.Instance.Player.ChangeWeapon(fireStaff);
	}
	private void FireStaffHit(Enemy target)
	{
		Buff fire = new Buff("FireStaffFire", 2f, target);
		Buff fireTimer = new Buff("FireStaffFireTimer", 0.5f, target, true);
		void FireStaffFireTimerRemove(Buff buff, IDamageable t)
		{
			print("hit fire");
			t.Hit(10, t);
			BuffManager.Instance.AddBuff(fireTimer, t);
		}
		fireTimer.OnRemoveBuff = FireStaffFireTimerRemove;
		fire.OnAddBuff = delegate
		{
			print("fire added");
			BuffManager.Instance.AddBuff(fireTimer, target);
		};
		fire.OnRemoveBuff = delegate
		{
			BuffManager.Instance.RemoveBuff(fireTimer, target, false);
		};
		BuffManager.Instance.AddBuff(fire, target);
	}
	public void meow()
    {

    }
}