using UnityEngine;

public class Ability: ScriptableObject
{
    public new string name;
    public string description;
    public Sprite icon;

    public float maxCooldown;
    [HideInInspector] public float cooldown;
    public float manaCost;
    public float damageMultiplier; // multiplier to magic damage

    /// <summary>
    /// can not be activated and have passive effect
    /// </summary>
    public bool passive = false;
    public bool targeted = false;

    [HideInInspector] public UnityEngine.UI.Text uiCooldownText;
    [HideInInspector] public UnityEngine.UI.Image uiCooldownImage;
    [HideInInspector] public AbilityData abilityData;

    private void OnEnable()
    {
        //abilityData = AbilityManager.Instance.FindAbilityData(name);
        cooldown = 0;
    }
    public bool canBeCasted()
    {
        if (cooldown > 0) // mana is checked in ability manager
        {            
            return false;
        }
        return true;
    }
    public virtual string GetDescription(Player caster)
    {
        return $@"{name}
{description}
Base damage: {caster.PlayerStats.MagicalDamage * damageMultiplier} ({damageMultiplier}x)
Cooldown: {maxCooldown}s
Mana cost: {manaCost}";
    }
    public virtual void OnUntargetedAbilityChoose(Player caster)
    {
        
    }
    /// <summary>
    /// primarily used for creating preview objects
    /// </summary>
    /// <param name="caster"></param>
    public virtual void OnTargetedAbilitySelect(Player caster)
    {

    }
    /// <summary>
    /// called when player clicks on the target position (base method in Ability class is required to be called)
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="target">noramlized vector with click position</param>
    public virtual void OnTargetedAbilityUse(Player caster, Vector2 target)
    {
        UseAbility(caster);
    }

    /// <summary>
    /// Primarily used for drawing lines and other visual effects. Called each frame
    /// </summary>
    /// <param name="caster"></param>
    public virtual void OnTargetedAbilityHold(Player caster)
    {

    }
    public virtual void OnTargetedAbilityCancel(Player caster)
    {

    }

    /// <summary>
    /// drain player's mana and set cooldown
    /// </summary>
    public void UseAbility(Player caster)
    {
        cooldown = maxCooldown;
        if (cooldown > 0)
        {
            AbilityManager.Instance.cooldowningAbilities.Add(this);
        }
        caster.SpendMana(manaCost);
    }
    public virtual void OnStart()
    {

    }
}
