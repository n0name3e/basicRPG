using UnityEngine;

[CreateAssetMenu(fileName = "Healing Potion", menuName = "Items/HealingPotion")]
public class HealingPotion : Item
{
    private void OnEnable()
    {
        //OnUse.AddListener(UseHealingPotion);
    }
    public void UseHealingPotion(Player player)
    {
        player.Heal(25);
    }
    public override void UseItem(Player player)
    {
        base.UseItem(player);
        UseHealingPotion(player);
    }
}
