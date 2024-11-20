using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item; // Assign this appropriately
    public Tooltip tooltip; // Reference to the Tooltip instance


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;
        // Immediately show the tooltip when the cursor hovers over the item
        System.Text.StringBuilder description = new System.Text.StringBuilder();
        description.Append($"<size=25>{item.Name}</size> <size=20>" + $"\n{item.Description} </size> \n");
        //tooltip.ShowTooltip($"<size=25>{item.Name}</size> <size=20>" + $"\n{item.Description} </size>");
        if (item is Equipment equipment)
        {
            Dictionary<StatType, float> statsDictionary
                = equipment.statsDictionary;
            foreach (KeyValuePair<StatType, float> stat in statsDictionary)
            {
                if (stat.Value != 0)
                {
                    description.Append($"<size=20> {stat.Key} +{stat.Value} </size> \n");
                }
            }
        }
        tooltip.ShowTooltip(description.ToString());
        //tooltip.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tooltip.transform.GetChild(1).GetComponent<Text>().preferredWidth);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide the tooltip when the cursor leaves the item
        tooltip.HideTooltip();
    }

}
