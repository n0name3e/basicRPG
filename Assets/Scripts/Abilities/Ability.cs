using UnityEngine;

public class Ability: ScriptableObject
{
    public new string name;
    public Sprite icon;

    public float maxCooldown;
    [HideInInspector] public float cooldown;
    public float manaCost;
    public float damageMultiplier; // multiplier to magic damage

    /// <summary>
    /// can not be activated and have passive effect
    /// </summary>
    public bool passive = false;

    [HideInInspector] public UnityEngine.UI.Text uiCooldownText;
    [HideInInspector] public UnityEngine.UI.Image uiCooldownImage;
    [HideInInspector] public AbilityData abilityData;

    private void OnEnable()
    {
        //abilityData = AbilityManager.Instance.FindAbilityData(name);
        cooldown = 0;
    }
    /// <summary>
    /// can be used if true and need conditions to activate if false!
    /// </summary>
    //public bool activated = true;
    /*public Ability(string name)
    {
        //this.name = name;
        //abilityData = AbilityManager.Instance.FindAbility(name);
        //manaCost = abilityData.manaCost;
    }*/
    public bool canBeCasted()
    {
        if (cooldown > 0) // mana is checked in ability manager
        {            
            return false;
        }
        return true;
    }
    public virtual void OnUntargetedAbilityChoose(Player caster)
    {
        
    }
    public virtual void OnStart()
    {

    }
    /*public void CopyDelegates(Ability originalAbility)
    {
        OnUntargetedAbilityChoose = originalAbility.OnUntargetedAbilityChoose;
        OnStart = originalAbility.OnStart;
    }

    //public delegate void UntargetedAbilityChoose();
    //public UntargetedAbilityChoose OnUntargetedAbilityChoose;


    public delegate void Init();
    public Init OnStart;*/

}
