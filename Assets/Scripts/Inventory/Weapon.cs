using UnityEngine;

//[System.Serializable]
//public class WeaponEvent: UnityEngine.Events.UnityEvent<GameObject> {}

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon", order = 2)]
public class Weapon: ScriptableObject
{
	public string Name;
	public Sprite icon;
    
	public float damage;
	public float attackCooldown; // can be multiplied by attack speed
	public bool isMelee = true;
	public delegate void AttackEvent(IDamageable target);
	public AttackEvent OnAttack;
	public delegate void HitEvent(IDamageable target);
	public HitEvent OnHit;
	public UnityEngine.Events.UnityEvent Onht;
}