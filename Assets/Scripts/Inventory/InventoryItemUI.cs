using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item; // Assign this appropriately
    public Tooltip tooltip; // Reference to the Tooltip instance

    public void OnPointerEnter(PointerEventData eventData)
    {        
        // Immediately show the tooltip when the cursor hovers over the item
        tooltip.ShowTooltip($"<size=45>{item.Name}</size> <size=40> \n\nPhysic Damage: {item.gainedStats.physicDamage}" +
            $"\n{item.Description} </size>");
        tooltip.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tooltip.transform.GetChild(1).GetComponent<Text>().preferredWidth);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the tooltip when the cursor leaves the item
        tooltip.HideTooltip();
    }

}
