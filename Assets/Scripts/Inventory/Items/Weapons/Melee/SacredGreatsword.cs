using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SacredGreatsword", menuName = "Weapons/SacredGreatsword", order = 3)]
public class SacredGreatsword : Sword
{
    public override void OnHit(Enemy enemy, Player hitter)
    {
        hitter.Heal(physicalDamage / 5f);
        enemy.Stun(3f);
    }
}
