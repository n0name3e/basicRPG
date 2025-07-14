using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Items/Sword", order = 2)]
public class Sword : Weapon
{
    public float physicalDamage;
    public float staminaCost;
    public float attackDuration;

    public GameObject prefab;

    public string swingAnimationName;
    public string thrustAnimationName;

    public virtual void OnHit(Enemy enemy, Player hitter)
    {

    }
}
