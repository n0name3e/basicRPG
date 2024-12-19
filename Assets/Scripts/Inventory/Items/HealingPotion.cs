using UnityEngine;

[CreateAssetMenu(fileName = "Healing Potion", menuName = "Items/HealingPotion")]
public class HealingPotion : Item
{
    private void OnEnable()
    {
        OnUse.AddListener(UseHealingPotion);
    }
    public void UseHealingPotion(GameObject player)
    {
        player.GetComponent<Player>().Heal(25);
    }
}
