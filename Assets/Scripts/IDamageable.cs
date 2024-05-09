public interface IDamageable
{
    /*float maxHealth { get; set; }
    float health { get; set; }
    float speed { get; set; }
    float attackSpeed { get; set; }*/
    void Hit(float damage, IDamageable sender);
    UnityEngine.Transform Transform { get; set; }
	System.Collections.Generic.List<Buff> buffs { get; set; }
}
