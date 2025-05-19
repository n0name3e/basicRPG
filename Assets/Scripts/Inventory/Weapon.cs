public class Weapon: Item
{    
	public float attackCooldown; // can be multiplied by attack speed

	public virtual void HitTarget(float damage, IDamageable target, IDamageable sender)
    {
		target.Hit(damage, sender);
    }
}