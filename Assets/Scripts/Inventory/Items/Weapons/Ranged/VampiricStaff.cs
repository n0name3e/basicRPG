[UnityEngine.CreateAssetMenu(fileName = "Vampiric Staff", menuName = "Weapons/VampiricStaff")]
public class VampiricStaff : Staff
{
    public float damageToHealthMultiplier = 0.25f;
    public override void HitTarget(float damage, IDamageable target, IDamageable sender)
    {
        float hpBeforeHit = target.Health;
        base.HitTarget(damage, target, sender);
        float hpAfterHit = target.Health;
        sender.Heal((hpBeforeHit - hpAfterHit) * damageToHealthMultiplier);
    }
}
