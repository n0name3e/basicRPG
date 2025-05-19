using UnityEngine;

[CreateAssetMenu(fileName = "AbilitiesDB", menuName = "Abilities/AbilitiesDatabase")]
public class Abilities : ScriptableObject
{
    public System.Collections.Generic.List<Ability> abilities = new System.Collections.Generic.List<Ability>();
}
