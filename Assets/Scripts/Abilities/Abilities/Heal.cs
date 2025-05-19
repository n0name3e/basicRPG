using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "heal", menuName = "Abilities/Heal")]
public class Heal : Ability
{
    public override void OnUntargetedAbilityChoose(Player caster)
    {
        base.OnUntargetedAbilityChoose(caster);
        caster.Heal(25);
    }
}
