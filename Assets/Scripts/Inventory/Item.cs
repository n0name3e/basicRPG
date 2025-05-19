using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemEvent: UnityEvent<GameObject> { } 

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item", order = 1)]
public class Item: ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    public Sprite image;

    //public ItemEvent OnUse;
    /*public Item(string name)
    {
        this.Name = name;
        gainedStats = new ItemStats();
    }*/
    public virtual void UseItem(Player player)
    {
        if (this is Equipment)  // sounds funny lol
        {
            player.inventory.EquipArmor((Equipment)this);
        }
        else if (this is Weapon)
        {
            if (this is Sword)
                player.inventory.ChangeMeleeWeapon((Sword)this);
            else if (this is Staff)
                player.inventory.ChangeMagicWeapon((Staff)this);
        }
        //else
            //OnUse?.Invoke(GameManager.Instance.Player.gameObject);
        player.inventory.RemoveItem(this);
        FindObjectOfType<Tooltip>().HideTooltip();
    }
}