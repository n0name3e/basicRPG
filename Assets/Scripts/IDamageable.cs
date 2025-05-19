public interface IDamageable
{
    float Health { get; set; }

    /*float maxHealth { get; set; }
    float speed { get; set; }
    float attackSpeed { get; set; }*/
    void Hit(float damage, IDamageable sender);
    void Heal(float amount);
    UnityEngine.Transform Transform { get; set; }
	System.Collections.Generic.List<Buff> buffs { get; set; }
}
