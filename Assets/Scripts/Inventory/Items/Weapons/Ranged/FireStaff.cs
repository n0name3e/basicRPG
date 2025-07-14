[UnityEngine.CreateAssetMenu(fileName = "FireStaff", menuName = "Weapons/FireStaff")]
public class FireStaff : Staff
{
	public float fireDamage = 10;
	public float fireDuration = 2;
	public float fireIntenstityTime = 0.5f;

    public override void HitTarget(float damage, IDamageable target, IDamageable sender)
    {
        base.HitTarget(damage, target, sender);
		Buff fire = new Buff("FireStaffFire", fireDuration, target);
		float timer = 0f;
		fire.OnFrameUpdate = delegate
		{
			timer += UnityEngine.Time.deltaTime;
			if (timer >= fireIntenstityTime)
            {
				target.Hit(fireDamage, sender);
				timer -= fireIntenstityTime;
            }
		};
		/*Buff fireTimer = new Buff("FireStaffFireTimer", fireIntenstityTime, target, true);
		void FireStaffFireTimerRemove(Buff buff, IDamageable t)
		{
			t.Hit(fireDamage, t);
			BuffManager.Instance.AddBuff(fireTimer, t);
		}
		fireTimer.OnRemoveBuff = FireStaffFireTimerRemove;
		fire.OnAddBuff = delegate
		{
			BuffManager.Instance.AddBuff(fireTimer, target);
		};
		fire.OnRemoveBuff = delegate
		{
			BuffManager.Instance.RemoveBuff(fireTimer, target, false);
		};*/
		BuffManager.Instance.AddBuff(fire, target);
	}
}
