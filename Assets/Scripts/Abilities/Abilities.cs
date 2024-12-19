using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    private static Abilities instance;

    private AbilityManager abilityManager;
    private Player player;

    public static Abilities Instance { get => instance; set => instance = value; }
    private void Awake()
    {
        instance = this;        
    }
    System.Collections.IEnumerator Start()
    {
        yield return null;
        player = FindObjectOfType<Player>();
        InitializeAbilities();    
    }
    private void InitializeAbilities()
    {
        Ability fireball = new Ability("fireball") { OnUntargetedAbilityChoose = UseFireball };
        
        player.Abilities.Add(fireball);
        AbilityManager.Instance.CreateAbilityButtons();
    }
    void UseFireball()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            enemy.Hit(player.PlayerStats.magicDamage * AbilityManager.Instance.FindAbility("fireball").damageMultiplier, player.GetComponent<IDamageable>());
        }
    }
}
