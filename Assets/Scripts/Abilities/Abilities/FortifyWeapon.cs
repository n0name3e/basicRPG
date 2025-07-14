using UnityEngine;

[CreateAssetMenu(fileName = "FortifyWeapon", menuName = "Abilities/FortifyWeapon")]
public class FortifyWeapon : Ability
{
    public override void OnUntargetedAbilityChoose(Player caster)
    {
        base.OnUntargetedAbilityChoose(caster);

        Buff damageBuff = new Buff("Fortify Weapon", 5f, caster, false);
        damageBuff.OnAddBuff = OnAddBuff;
        damageBuff.OnRemoveBuff = OnRemoveBuff;

        BuffManager.Instance.AddBuff(damageBuff, caster);
    }
    private void OnAddBuff(Buff buff, IDamageable target)
    {
        MonoBehaviour.print("add");
        target.Transform.GetComponent<PlayerStats>().ModifyStat(StatType.PhysicalDamage, 20f);
    }
    private void OnRemoveBuff(Buff buff, IDamageable target)
    {
        MonoBehaviour.print("remove");
        target.Transform.GetComponent<PlayerStats>().ModifyStat(StatType.PhysicalDamage, -20f);
    }
}
