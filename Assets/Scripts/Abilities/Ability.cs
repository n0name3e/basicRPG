public class Ability
{
    public string name;
    public float cooldown;
    public float manaCost;
    //public float damageMultiplier; // multiplier to magic damage

    /// <summary>
    /// can not be activated and have passive effect
    /// </summary>
    public bool passive = false;

    public UnityEngine.UI.Text uiCooldownText;
    public UnityEngine.UI.Image uiCooldownImage;

    /// <summary>
    /// can be used if true and need conditions to activate if false!
    /// </summary>
    //public bool activated = true;
    public Ability(string name)
    {
        this.name = name;
        abilityData = AbilityManager.Instance.FindAbility(name);
        manaCost = abilityData.manaCost;
    }
    public bool canBeCasted()
    {
        //if (!activated) return false;
        //isCharges = AbilityManager.Instance.FindAbility(name).chargesConditions.Count > 0;
        //if (isCharges && charges <= 0) return false;
        if (cooldown > 0)
        {
            return false;
        }
        return true;
    }
    public void CopyDelegates(Ability originalAbility)
    {
        OnUntargetedAbilityChoose = originalAbility.OnUntargetedAbilityChoose;
        OnStart = originalAbility.OnStart;
    }
    public AbilityData abilityData;


    public delegate void UntargetedAbilityChoose();
    public UntargetedAbilityChoose OnUntargetedAbilityChoose;


    public delegate void Init();
    public Init OnStart;

}
