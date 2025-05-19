using UnityEngine;

[CreateAssetMenu(fileName = "fireball", menuName = "Abilities/Fireball")]
public class Fireball: Ability
{
    public override void OnUntargetedAbilityChoose(Player caster)
    {
        base.OnUntargetedAbilityChoose(caster);
        MonoBehaviour.print("vasyl chaklun");
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.Hit(caster.PlayerStats.MagicalDamage * damageMultiplier, caster.GetComponent<IDamageable>());
        }
    }
}
