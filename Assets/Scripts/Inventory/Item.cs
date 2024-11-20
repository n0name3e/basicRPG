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
    public ItemEvent OnUse;
    /*public Item(string name)
    {
        this.Name = name;
        gainedStats = new ItemStats();
    }*/
    public void UseItem()
    {
        if (this is Equipment)  // sounds funny lol, i want to have class Sparta 
        {
            Inventory.Instance.EquipArmor((Equipment)this);
        }
        else
            OnUse?.Invoke(GameManager.Instance.Player.gameObject);
        Inventory.Instance.RemoveItem(this);
        FindObjectOfType<Tooltip>().HideTooltip();
    }
}
