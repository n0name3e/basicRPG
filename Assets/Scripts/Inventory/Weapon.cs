using UnityEngine;

[System.Serializable]
public class WeaponEvent: UnityEngine.Events.UnityEvent<GameObject> {}

[CreateAssetMenu(fileName = "Weapon", menuName = "Items", order = 2)]
public class Weapon: ScriptableObject
{
	public string Name;
	public Sprite icon;
    
	public float damage;
	public float attackCooldown; // can be multiplied by attack speed
	public delegate void AttackEvent();
	public AttackEvent OnAttack;
	public delegate void HitEvent(Enemy target);
	public HitEvent OnHit;
}